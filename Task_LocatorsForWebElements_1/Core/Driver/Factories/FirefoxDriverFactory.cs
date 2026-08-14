using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace CoreLayer.WebDriver.Factories
{
    public sealed class FirefoxDriverFactory : BaseDriverFactory<FirefoxOptions>
    {
        public FirefoxDriverFactory(BrowserSettings settings)
            : base(settings)
        {
        }

        protected override FirefoxOptions CreateConfiguredOptions()
        {
            FirefoxOptions options = ConfigureOptions(
                new FirefoxOptions(),
                (configuredOptions, argument) => configuredOptions.AddArgument(argument),
                AddFirefoxPreference);

            options.SetPreference("browser.download.dir", Settings.Downloads.Directory);
            options.SetPreference("browser.download.folderList", 2);
            options.SetPreference("browser.download.useDownloadDir", true);

            return options;
        }

        protected override IWebDriver StartDriver(FirefoxOptions options)
        {
            return new FirefoxDriver(options);
        }

        private static void AddFirefoxPreference(
            FirefoxOptions options,
            string key,
            object value)
        {
            switch (value)
            {
                case bool boolValue:
                    options.SetPreference(key, boolValue);
                    break;
                case int intValue:
                    options.SetPreference(key, intValue);
                    break;
                default:
                    options.SetPreference(key, value.ToString() ?? string.Empty);
                    break;
            }
        }
    }
}
