using TestProject.Configuration;

namespace BusinessActionLayer.Configuration
{
    public static class UiTestConfig
    {
        public static TestUISettings Settings { get; } = TestConfig.Load<TestUISettings>("appsettings.chrome.json");
    }
}
