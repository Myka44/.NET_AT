using TestProject.PageObjects;

namespace TestFramework.Core.BrowserUtils
{
    public class ScreenshotMaker
    {
        private readonly CustomWebDriver _driver;
        private readonly string _screenshotDirectory;

        public ScreenshotMaker(CustomWebDriver driver, string screenshotDirectory)
        {
            _driver = driver;
            _screenshotDirectory = screenshotDirectory;
        }

        public string TakeBrowserScreenshot()
        {
            Directory.CreateDirectory(_screenshotDirectory);

            var now = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff");
            var screenshotPath = Path.Combine(_screenshotDirectory, $"Display_{now}.png");

            _driver.TakeScreenshot().SaveAsFile(screenshotPath);

            return screenshotPath;
        }
    }
}
