using Microsoft.Extensions.Configuration;

namespace TestProject.Configuration
{
    public static class TestConfig
    {
        public static TSettings Load<TSettings>(string configurationFile) where TSettings : new()
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(configurationFile);

            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(
                    configurationFile,
                    optional: false,
                    reloadOnChange: false)
                .Build();

            return configuration.Get<TSettings>() ?? new TSettings();
        }
    }
}
