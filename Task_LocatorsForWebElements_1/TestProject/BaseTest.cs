using CoreLayer.WebDriver;
using log4net;
using log4net.Config;
using log4net.Core;
using log4net.Repository.Hierarchy;
using TestProject.Configuration;
using TestProject.PageObjects;
using TestFramework.Core.BrowserUtils;

namespace TestProject
{
    public abstract class BaseTest : IDisposable
    {
        protected readonly CustomWebDriver _driver;
        protected readonly string DownloadDirectory;
        protected TestSettings Settings => TestConfig.Settings;

        private readonly ScreenshotMaker _screenshotMaker;

        protected ILog Log
        {
            get { return LogManager.GetLogger(this.GetType()); }
        }

        protected BaseTest()
        {
            ConfigureLogging(Settings.Logging.MinLevel);

            DownloadDirectory = CreateDownloadDirectory(Settings.Browser.Downloads.Directory);

            var driver = WebDriverFactory.Instance.CreateWebDriver(CreateBrowserSettings(DownloadDirectory));

            if (Settings.Browser.Maximize)
            {
                driver.Manage().Window.Maximize();
            }

            _driver = new CustomWebDriver(driver, TimeSpan.FromSeconds(Settings.Browser.ExplicitWaitSeconds));

            _screenshotMaker = new ScreenshotMaker(_driver, Settings.Screenshots.Directory);
        }

        private static void ConfigureLogging(string minLevel)
        {
            XmlConfigurator.Configure(new FileInfo(Path.Combine(AppContext.BaseDirectory, "Log.config")));

            var hierarchy = (Hierarchy)LogManager.GetRepository();
            var level = hierarchy.LevelMap[minLevel.ToUpperInvariant()] ?? Level.Info;

            hierarchy.Root.Level = level;
            hierarchy.RaiseConfigurationChanged(EventArgs.Empty);
        }

        public void Dispose()
        {
            Log.Info("Test dispose");
            _driver.Quit();
            if (Directory.Exists(DownloadDirectory))
            {
                Directory.Delete(DownloadDirectory, true);
            }
        }

        protected void ExecuteTest(Action testAction)
        {
            try
            {
                testAction();
            }
            catch (Exception ex)
            {
                string screenshotPath = _screenshotMaker.TakeBrowserScreenshot();
                Log.Error($"Test failed, screenshot saved to: {screenshotPath}", ex);
                throw;
            }
        }

        private static string CreateDownloadDirectory(string baseDownloadDirectory)
        {
            string downloadDirectory = Path.GetFullPath(Path.Combine(baseDownloadDirectory, Guid.NewGuid().ToString()));
            Directory.CreateDirectory(downloadDirectory);

            return downloadDirectory;
        }

        private BrowserSettings CreateBrowserSettings(string downloadDirectory)
        {
            return new BrowserSettings
            {
                Name = Settings.Browser.Name,
                ExplicitWaitSeconds = Settings.Browser.ExplicitWaitSeconds,
                Maximize = Settings.Browser.Maximize,
                Downloads =
                {
                    Directory = downloadDirectory
                },
                Options =
                {
                    Arguments = new List<string>(Settings.Browser.Options.Arguments),
                    Preferences = new Dictionary<string, string>(Settings.Browser.Options.Preferences)
                }
            };
        }
    }
}
