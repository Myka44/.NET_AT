using CoreLayer.WebDriver;

namespace TestProject.Configuration
{
    public class TestSettings
    {
        public EnvironmentSettings Environment { get; set; } = new();
        public BrowserSettings Browser { get; set; } = new();
        public LoggingSettings Logging { get; set; } = new();

        public ScreenshotSettings Screenshots { get; set; } = new();
    }

    public class EnvironmentSettings
    {
        public string BaseUrl { get; set; } = "https://www.epam.com/";
    }

    public class LoggingSettings
    {
        public string MinLevel { get; set; } = "Info";
    }

    public class ScreenshotSettings
    {
        public string Directory { get; set; } = "Screenshots";
    }
}
