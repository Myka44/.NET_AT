using OpenQA.Selenium;

namespace CoreLayer.WebDriver.Factories
{
    public abstract class BaseDriverFactory<TOptions> : IWebDriverFactory
        where TOptions : DriverOptions
    {
        protected BrowserSettings Settings { get; }

        protected BaseDriverFactory(BrowserSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IWebDriver CreateDriver()
        {
            TOptions options = CreateConfiguredOptions();
            return StartDriver(options);
        }

        protected abstract TOptions CreateConfiguredOptions();

        protected abstract IWebDriver StartDriver(TOptions options);

        protected TOptions ConfigureOptions(
            TOptions options,
            Action<TOptions, string> addArgument,
            Action<TOptions, string, object> addPreference)
        {
            foreach (string argument in Settings.Options.Arguments.Where(
                         argument => !string.IsNullOrWhiteSpace(argument)))
            {
                addArgument(options, argument);
            }

            foreach (var preference in Settings.Options.Preferences)
            {
                addPreference(
                    options,
                    preference.Key,
                    ConvertPreferenceValue(preference.Value));
            }

            return options;
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
    }
}
