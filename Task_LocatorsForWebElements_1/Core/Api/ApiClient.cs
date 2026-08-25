using log4net;
using RestSharp;

namespace CoreLayer.Api
{
    public sealed class ApiClient : IDisposable
    {
        private readonly RestClient _client;
        private readonly ILog _log = LogManager.GetLogger(typeof(ApiClient));

        public ApiClient(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentException("API base URL must be provided.", nameof(baseUrl));
            }

            _client = new RestClient(new RestClientOptions(baseUrl)
            {
                SetErrorExceptionOnUnsuccessfulStatusCode = false
            });
        }

        public async Task<RestResponse> ExecuteAsync(
            RestRequest request,
            CancellationToken cancellationToken = default)
        {
            LogRequest(request);
            
            RestResponse response = await _client.ExecuteAsync(request, cancellationToken);

            LogResponse(request, response);
            return response;
        }

        public async Task<RestResponse<T>> ExecuteAsync<T>(
            RestRequest request,
            CancellationToken cancellationToken = default)
            where T : notnull
        {
            LogRequest(request);

            RestResponse<T> response = await _client.ExecuteAsync<T>(request, cancellationToken);

            LogResponse(request, response);
            return response;
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        private void LogRequest(RestRequest request)
        {
            _log.Info($"Sending {request.Method} API request to '{request.Resource}'.");
        }

        private void LogResponse(RestRequest request, RestResponse response)
        {
            string message =
                $"Received {(int)response.StatusCode} {response.StatusCode} " +
                $"for {request.Method} '{request.Resource}'.";

            if (response.ErrorException is null && string.IsNullOrWhiteSpace(response.ErrorMessage))
            {
                _log.Info(message);
                return;
            }

            _log.Error($"{message} {response.ErrorMessage}", response.ErrorException);
        }
    }
}
