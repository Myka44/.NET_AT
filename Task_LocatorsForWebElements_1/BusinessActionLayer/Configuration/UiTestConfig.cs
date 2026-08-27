using TestProject.Configuration;

namespace BusinessActionLayer.Configuration
{
    public static class UiTestConfig
    {
        public static TestUISettings Settings { get; } = LoadSettings();

        private static TestUISettings LoadSettings()
        {
            string browser = Environment.GetEnvironmentVariable("UI_BROWSER")?
                .Trim()
                .ToLowerInvariant() ?? "chrome";

            string configurationFile = browser switch
            {
                "chrome" => "appsettings.chrome.json",
                "edge" => "appsettings.edge.json",
                "firefox" => "appsettings.firefox.json",
                _ => throw new InvalidOperationException(
                    $"Unsupported UI browser '{browser}'.")
            };

            TestUISettings settings = TestConfig.Load<TestUISettings>(configurationFile);
            AddCiHeadlessArgument(settings);

            return settings;
        }

        private static void AddCiHeadlessArgument(TestUISettings settings)
        {
            if (!string.Equals(
                    Environment.GetEnvironmentVariable("CI"),
                    "true",
                    StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string headlessArgument = settings.Browser.Name.Equals(
                "firefox",
                StringComparison.OrdinalIgnoreCase)
                ? "-headless"
                : "--headless=new";

            if (!settings.Browser.Options.Arguments.Contains(
                    headlessArgument,
                    StringComparer.OrdinalIgnoreCase))
            {
                settings.Browser.Options.Arguments.Add(headlessArgument);
            }
        }
    }
}
