using RestSharp;

namespace CoreLayer.Api
{
    public sealed class ApiRequestBuilder
    {
        private readonly List<Action<RestRequest>> _configurationSteps = new();
        private string? _resource;
        private Method _method = Method.Get;

        public ApiRequestBuilder WithResource(string resource)
        {
            _resource = resource;
            return this;
        }

        public ApiRequestBuilder WithMethod(Method method)
        {
            _method = method;
            return this;
        }

        public ApiRequestBuilder WithHeader(string name, string value)
        {
            _configurationSteps.Add(request => request.AddHeader(name, value));
            return this;
        }

        public ApiRequestBuilder WithQueryParameter(string name, string value)
        {
            _configurationSteps.Add(request => request.AddQueryParameter(name, value));
            return this;
        }

        public ApiRequestBuilder WithJsonBody<T>(T body)
            where T : class
        {
            _configurationSteps.Add(request => request.AddJsonBody(body));
            return this;
        }

        public RestRequest Build()
        {
            if (string.IsNullOrWhiteSpace(_resource))
            {
                throw new InvalidOperationException("A request resource must be configured before building the request.");
            }

            var request = new RestRequest(_resource, _method);

            foreach (Action<RestRequest> configure in _configurationSteps)
            {
                configure(request);
            }

            return request;
        }
    }
}
