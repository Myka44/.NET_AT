using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace CoreLayer.WebDriver.Factories
{
    public sealed class EdgeDriverFactory : BaseDriverFactory<EdgeOptions>
    {
        public EdgeDriverFactory(BrowserSettings settings)
            : base(settings)
        {
        }

        protected override EdgeOptions CreateConfiguredOptions()
        {
            EdgeOptions options = ConfigureOptions(
                new EdgeOptions(),
                (configuredOptions, argument) => configuredOptions.AddArgument(argument),
                (configuredOptions, key, value) =>
                    configuredOptions.AddUserProfilePreference(key, value));

            options.AddUserProfilePreference(
                "download.default_directory",
                Settings.Downloads.Directory);
            options.AddUserProfilePreference("download.prompt_for_download", false);

            return options;
        }

        protected override IWebDriver StartDriver(EdgeOptions options)
        {
            return new EdgeDriver(options);
        }
    }
}
