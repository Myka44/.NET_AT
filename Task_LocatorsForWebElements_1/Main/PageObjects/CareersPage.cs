using OpenQA.Selenium;

namespace TestProject.PageObjects
{
    public class CareersPage : BasePage
    {
        private readonly By _startYourSearchLocator = By.CssSelector("a[href*='careers.epam.com/en/jobs']");

        public CareersPage(CustomWebDriver driver) : base(driver) { }

        public JobPage StartYourSearch()
        {
            Log.Info("Starting careers job search");
            CustomDriver.ClickWhenReady(_startYourSearchLocator);
            CustomDriver.AcceptCookiesIfPresent();
            return new JobPage(CustomDriver);
        }
    }
}
