namespace CoreLayer.WebDriver.Factories
{
    public sealed class WebDriverFactoryProvider
    {
        private static readonly WebDriverFactoryProvider _instance = new();

        public static WebDriverFactoryProvider Instance => _instance;

        private WebDriverFactoryProvider()
        {
        }

        public IWebDriverFactory CreateFactory(BrowserSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            return settings.Name.Trim().ToLowerInvariant() switch
            {
                "chrome" => new ChromeDriverFactory(settings),
                "edge" => new EdgeDriverFactory(settings),
                "firefox" => new FirefoxDriverFactory(settings),
                _ => throw new ArgumentException(
                    $"Unsupported browser: {settings.Name}",
                    nameof(settings))
            };
        }
    }
}
