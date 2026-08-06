using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace CoreLayer.WebDriver
{
    public class WebDriverFactory
    {
        private static readonly WebDriverFactory _instance = new();
        public static WebDriverFactory Instance => _instance;

        private static readonly List<BrowserType> BrowserTypes = new()
        {
            BrowserType.Chrome,
            BrowserType.Edge,
            BrowserType.Firefox
        };

        private WebDriverFactory() { }

        public IWebDriver CreateWebDriver(BrowserSettings settings)
        {
            BrowserType browserType = BrowserTypes.First(x => x.Name.Equals(settings.Name, StringComparison.OrdinalIgnoreCase));
            return CreateWebDriver(browserType, settings);
        }

        public IWebDriver CreateWebDriver(BrowserType browserType)
        {
            return CreateWebDriver(browserType, new BrowserSettings());
        }

        public IWebDriver CreateWebDriverByString(string name)
        {
            BrowserType browserType = BrowserTypes.First(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return CreateWebDriver(browserType);
        }

        private IWebDriver CreateWebDriver(BrowserType browserType, BrowserSettings settings)
        {
            if (browserType == BrowserType.Chrome)
            {
                return new ChromeDriver(CreateChromeOptions(settings));
            }

            if (browserType == BrowserType.Edge)
            {
                return new EdgeDriver(CreateEdgeOptions(settings));
            }

            if (browserType == BrowserType.Firefox)
            {
                return new FirefoxDriver(CreateFirefoxOptions(settings));
            }

            throw new ArgumentException($"Unsupported browser type: {browserType.Name}");
        }

        private static ChromeOptions CreateChromeOptions(BrowserSettings settings)
        {
            var options = ConfigureOptions(
                new ChromeOptions(),
                settings.Options,
                (options, argument) => options.AddArgument(argument),
                (options, key, value) => options.AddUserProfilePreference(key, value));

            options.AddUserProfilePreference("download.default_directory", settings.Downloads.Directory);
            options.AddUserProfilePreference("download.prompt_for_download", false);

            return options;
        }

        private static EdgeOptions CreateEdgeOptions(BrowserSettings settings)
        {
            var options = ConfigureOptions(
                new EdgeOptions(),
                settings.Options,
                (options, argument) => options.AddArgument(argument),
                (options, key, value) => options.AddUserProfilePreference(key, value));

            options.AddUserProfilePreference("download.default_directory", settings.Downloads.Directory);
            options.AddUserProfilePreference("download.prompt_for_download", false);

            return options;
        }

        private static FirefoxOptions CreateFirefoxOptions(BrowserSettings settings)
        {
            var options = ConfigureOptions(
                new FirefoxOptions(),
                settings.Options,
                (options, argument) => options.AddArgument(argument),
                AddFirefoxPreference);

            options.SetPreference("browser.download.dir", settings.Downloads.Directory);
            options.SetPreference("browser.download.folderList", 2);
            options.SetPreference("browser.download.useDownloadDir", true);

            return options;
        }

        private static T ConfigureOptions<T>(
            T options,
            BrowserOptionsSettings settings,
            Action<T, string> addArgument,
            Action<T, string, object> addPreference)
        {
            foreach (string argument in settings.Arguments.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                addArgument(options, argument);
            }

            foreach (var preference in settings.Preferences)
            {
                addPreference(options, preference.Key, ConvertPreferenceValue(preference.Value));
            }

            return options;
        }

        private static void AddFirefoxPreference(FirefoxOptions options, string key, object value)
        {
            if (value is bool boolValue)
            {
                options.SetPreference(key, boolValue);
            }
            else if (value is int intValue)
            {
                options.SetPreference(key, intValue);
            }
            else
            {
                options.SetPreference(key, value.ToString() ?? string.Empty);
            }
        }

        private static object ConvertPreferenceValue(string value)
        {
            if (bool.TryParse(value, out bool boolValue))
            {
                return boolValue;
            }

            if (int.TryParse(value, out int intValue))
            {
                return intValue;
            }

            return value;
        }

        public record BrowserType(string Name)
        {
            public static readonly BrowserType Chrome = new("chrome");
            public static readonly BrowserType Edge = new("edge");
            public static readonly BrowserType Firefox = new("firefox");
        }
    }
}
