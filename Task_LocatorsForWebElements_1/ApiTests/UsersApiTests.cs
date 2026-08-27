using System.Net;
using BusinessLayer.Models.Api;
using CoreLayer.Api;
using log4net;
using NUnit.Framework;
using RestSharp;
using TestLayer.Utils;
using TestProject.Configuration;
using NUnitAssert = NUnit.Framework.Assert;

namespace TestLayer.ApiTests
{
    [TestFixture]
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    [Parallelizable(ParallelScope.All)] // look into
    [Category("API")]
    public sealed class UsersApiTests
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UsersApiTests));
        private static readonly TestApiSettings Settings =
            TestConfig.Load<TestApiSettings>("appsettings.api.json");

        [OneTimeSetUp]
        public static void ConfigureLogging()
        {
            LoggingUtils.Configure(Settings.Logging.MinLevel);
        }

        [Test]
        public async Task GetUsers_ReturnsUsersWithRequiredInformation()
        {
            Log.Info("Validating that the users list contains all required user information.");

            using var client = new ApiClient(Settings.BaseUrl);
            RestRequest request = CreateRequest(Settings.UsersEndpoint, Method.Get);

            RestResponse<List<User>> response = await client.ExecuteAsync<List<User>>(request);

            AssertSuccessfulResponse(response, HttpStatusCode.OK);
            List<User> users = RequireData(response);

            NUnitAssert.That(users, Is.Not.Empty, "The response must contain users.");

            NUnitAssert.That(users, Has.All.Matches<User>(user =>
                user.Id > 0 &&
                !string.IsNullOrWhiteSpace(user.Name) &&
                !string.IsNullOrWhiteSpace(user.Username) &&
                !string.IsNullOrWhiteSpace(user.Email) &&
                user.Address is not null &&
                !string.IsNullOrWhiteSpace(user.Phone) &&
                !string.IsNullOrWhiteSpace(user.Website) &&
                user.Company is not null));
        }

        //add parameter for expected result
        [TestCase("application/json; charset=utf-8")]
        public async Task GetUsers_ReturnsExpectedContentTypeHeader(string contentType)
        {
            Log.Info("Validating the users response Content-Type header.");

            using var client = new ApiClient(Settings.BaseUrl);
            RestRequest request = CreateRequest(Settings.UsersEndpoint, Method.Get);


            RestResponse response = await client.ExecuteAsync(request);

            AssertSuccessfulResponse(response, HttpStatusCode.OK);
            string? contentTypeHeader = response.ContentHeaders?
                .FirstOrDefault(header => string.Equals(
                    header.Name,
                    "Content-Type",
                    StringComparison.OrdinalIgnoreCase))
                ?.Value?.ToString();

            NUnitAssert.That(contentTypeHeader, Is.Not.Null.And.Not.Empty,
                "The Content-Type header must exist.");
            NUnitAssert.That(contentTypeHeader, Is.EqualTo(contentType),
                "The Content-Type header has an unexpected value.");
        }

        [Test]
        public async Task GetUsers_ReturnsTenValidUsersWithUniqueIds()
        {
            Log.Info("Validating the users count, unique IDs, names, usernames, and companies.");

            using var client = new ApiClient(Settings.BaseUrl);
            RestRequest request = CreateRequest(Settings.UsersEndpoint, Method.Get);

            RestResponse<List<User>> response = await client.ExecuteAsync<List<User>>(request);

            AssertSuccessfulResponse(response, HttpStatusCode.OK);
            List<User> users = RequireData(response);

            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(users, Has.Count.EqualTo(10),
                    "The response must contain exactly 10 users.");
                NUnitAssert.That(users.Select(user => user.Id).Distinct().Count(),
                    Is.EqualTo(users.Count), "Every user ID must be unique.");
                NUnitAssert.That(users.All(user => !string.IsNullOrWhiteSpace(user.Name)), Is.True,
                    "Every user must have a name.");
                NUnitAssert.That(users.All(user => !string.IsNullOrWhiteSpace(user.Username)), Is.True,
                    "Every user must have a username.");
                NUnitAssert.That(
                    users.All(user => user.Company is not null &&
                                      !string.IsNullOrWhiteSpace(user.Company.Name)), Is.True,
                    "Every user must have a company with a name.");
            });
        }
        //Follow AAA
        [Test]
        public async Task PostUser_CreatesUserAndReturnsId()
        {
            Log.Info("Validating that a user can be created with name and username fields.");

            var newUser = new CreateUserRequest
            {
                Name = "Test User",
                Username = "test.user"
            };

            using var client = new ApiClient(Settings.BaseUrl);
            RestRequest request = new ApiRequestBuilder()
                .WithResource(Settings.UsersEndpoint)
                .WithMethod(Method.Post)
                .WithJsonBody(newUser)
                .Build();

            RestResponse<User> response = await client.ExecuteAsync<User>(request);

            AssertSuccessfulResponse(response, HttpStatusCode.Created);
            User createdUser = RequireData(response);

            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(response.Content, Is.Not.Null.And.Not.Empty,
                    "The create-user response must not be empty.");
                NUnitAssert.That(createdUser.Id, Is.GreaterThan(0),
                    "The created user response must contain an ID.");
            });
        }

        [Test]
        public async Task GetInvalidEndpoint_ReturnsNotFoundWithoutClientErrors()
        {
            Log.Info("Validating that a missing resource returns 404 Not Found.");

            using var client = new ApiClient(Settings.BaseUrl);
            RestRequest request = CreateRequest(Settings.InvalidEndpoint, Method.Get);

            RestResponse response = await client.ExecuteAsync(request);

            AssertCompletedWithoutClientErrors(response);
            NUnitAssert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound),
                "The missing resource must return 404 Not Found.");
        }

        private static RestRequest CreateRequest(string resource, Method method)
        {
            return new ApiRequestBuilder()
                .WithResource(resource)
                .WithMethod(method)
                .Build();
        }

        private static void AssertSuccessfulResponse(RestResponse response, HttpStatusCode expectedStatusCode)
        {
            AssertCompletedWithoutClientErrors(response);
            NUnitAssert.Multiple(() =>
            {
                NUnitAssert.That(response.StatusCode, Is.EqualTo(expectedStatusCode));
                NUnitAssert.That(response.IsSuccessful, Is.True,
                    "The response must be successful.");
            });
        }

        private static void AssertCompletedWithoutClientErrors(RestResponse response)
        {
            NUnitAssert.That(
                response.ResponseStatus,
                Is.EqualTo(ResponseStatus.Completed),
                () => $"HTTP exchange failed: " + $"{response.ErrorMessage ?? response.ErrorException?.Message}");
        }
        private static T RequireData<T>(RestResponse<T> response)
        {
            NUnitAssert.That(response.Data, Is.Not.Null,
                "The response body must deserialize successfully.");

            return response.Data!;
        }
    }
}
