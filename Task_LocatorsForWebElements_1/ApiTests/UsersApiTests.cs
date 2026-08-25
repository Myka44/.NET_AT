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

            //use Assert.That
            //Assert.That() Is.EquivalentTo(
            foreach (User user in users) 
            {
                NUnitAssert.Multiple(() =>
                {
                    NUnitAssert.That(user.Id, Is.GreaterThan(0), "User ID must be present.");
                    NUnitAssert.That(user.Name, Is.Not.Empty, "User name must be present.");
                    NUnitAssert.That(user.Username, Is.Not.Empty, "Username must be present.");
                    NUnitAssert.That(user.Email, Is.Not.Empty, "Email must be present.");
                    NUnitAssert.That(user.Address, Is.Not.Null, "Address must be present.");
                    NUnitAssert.That(user.Phone, Is.Not.Empty, "Phone must be present.");
                    NUnitAssert.That(user.Website, Is.Not.Empty, "Website must be present.");
                    NUnitAssert.That(user.Company, Is.Not.Null, "Company must be present.");
                });
            }

            Log.Info($"Validated required information for {users.Count} users.");
        }

        //add parameter for expected result
        [Test]
        public async Task GetUsers_ReturnsExpectedContentTypeHeader()
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
            NUnitAssert.That(contentTypeHeader, Is.EqualTo("application/json; charset=utf-8"),
                "The Content-Type header has an unexpected value.");

            Log.Info($"Validated Content-Type header '{contentTypeHeader}'.");
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

            Log.Info("Validated all 10 users and their unique IDs.");
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

            //Log.Info($"Validated created user ID {createdUser.Id}.");
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
            //No methods after assertions
            //Log.Info("Validated 404 Not Found response without client execution errors.");
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
