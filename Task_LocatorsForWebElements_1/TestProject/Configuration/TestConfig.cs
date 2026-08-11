using Microsoft.Extensions.Configuration;

namespace TestProject.Configuration
{
    public static class TestConfig
    {
        public static TestSettings Settings { get; } = LoadSettings();

        private static TestSettings LoadSettings()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.firefox.json")
                .Build();

            return configuration.Get<TestSettings>() ?? new TestSettings();
        }
    }
}
