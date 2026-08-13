using CoreLayer.WebDriver;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using CoreLayer.WebDriver.Factories;
using Reqnroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFramework.Core.BrowserUtils;
using TestLayer.SessionContext;
using TestLayer.Utils;
using TestProject.Configuration;
using TestProject.PageObjects;

namespace TestLayer.Hooks
{

    [Binding]
    public class TestHooks
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly TestSessionContext _testSessionContext;

        public TestHooks(ScenarioContext scenarioContext, TestSessionContext testSessionContext)
        {
            _scenarioContext = scenarioContext;
            _testSessionContext = testSessionContext;
        }



        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            LoggingUtils.Configure(TestConfig.Settings.Logging.MinLevel);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _testSessionContext.DownloadDirectory = DownloadUtils.CreateDirectory(_testSessionContext.Settings.Browser.Downloads.Directory);

            BrowserSettings browserSettings = CreateBrowserSettings(
                _testSessionContext.DownloadDirectory);

            IWebDriverFactory browserFactory = WebDriverFactoryProvider.Instance
                .CreateFactory(browserSettings);

            var driver = browserFactory.CreateDriver();

            if (_testSessionContext.Settings.Browser.Maximize)
            {
                driver.Manage().Window.Maximize();
            }

            _testSessionContext.Driver = new CustomWebDriver(driver, TimeSpan.FromSeconds(_testSessionContext.Settings.Browser.ExplicitWaitSeconds));

            _testSessionContext.ScreenshotMaker = new ScreenshotMaker(_testSessionContext.Driver, _testSessionContext.Settings.Screenshots.Directory);
        }


        [AfterScenario(Order = 0)]
        public void TakeScreenshotOnFailure()
        {
            if (_scenarioContext.TestError is not null)
            {
                _testSessionContext.ScreenshotMaker.TakeBrowserScreenshot();
            }
        }

        [AfterScenario(Order = 10)]
        public void CloseBrowser()
        {
            _testSessionContext.Driver?.Quit();

            if (Directory.Exists(_testSessionContext.DownloadDirectory))
            {
                Directory.Delete(_testSessionContext.DownloadDirectory, recursive: true);
            }
        }

        private BrowserSettings CreateBrowserSettings(string downloadDirectory)
        {
            return new BrowserSettings
            {
                Name = _testSessionContext.Settings.Browser.Name,
                ExplicitWaitSeconds = _testSessionContext.Settings.Browser.ExplicitWaitSeconds,
                Maximize = _testSessionContext.Settings.Browser.Maximize,
                Downloads =
                {
                    Directory = downloadDirectory
                },
                Options =
                {
                    Arguments = new List<string>(_testSessionContext.Settings.Browser.Options.Arguments),
                    Preferences = new Dictionary<string, string>(_testSessionContext.Settings.Browser.Options.Preferences)
                }
            };
        }
    }
}
