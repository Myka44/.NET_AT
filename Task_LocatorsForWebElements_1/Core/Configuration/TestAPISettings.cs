using CoreLayer.Configuration;
using CoreLayer.WebDriver;

namespace TestProject.Configuration
{
    public class TestApiSettings
    {
        public string BaseUrl { get; set; } = "https://jsonplaceholder.typicode.com/";
        public string UsersEndpoint { get; set; } = "users";
        public string InvalidEndpoint { get; set; } = "invalidendpoint";
        public LoggingSettings Logging { get; set; } = new();
    }

}
