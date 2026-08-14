using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CoreLayer.WebDriver.Factories
{
    public sealed class ChromeDriverFactory : BaseDriverFactory<ChromeOptions>
    {
        public ChromeDriverFactory(BrowserSettings settings)
            : base(settings)
        {
        }

        protected override ChromeOptions CreateConfiguredOptions()
        {
            ChromeOptions options = ConfigureOptions(
                new ChromeOptions(),
                (configuredOptions, argument) => configuredOptions.AddArgument(argument),
                (configuredOptions, key, value) =>
                    configuredOptions.AddUserProfilePreference(key, value));

            options.AddUserProfilePreference(
                "download.default_directory",
                Settings.Downloads.Directory);
            options.AddUserProfilePreference("download.prompt_for_download", false);

            return options;
        }

        protected override IWebDriver StartDriver(ChromeOptions options)
        {
            return new ChromeDriver(options);
        }
    }
}
