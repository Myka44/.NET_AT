using OpenQA.Selenium;

namespace TestProject.PageObjects
{
    public class CareersPage : BasePage
    {
        private readonly By _startYourSearchLocator = By.CssSelector("a.button-body");

        public CareersPage(CustomWebDriver driver) : base(driver) { }

        public JobPage StartYourSearch()
        {
            Driver.ClickWhenReady(_startYourSearchLocator);
            return new JobPage(Driver);
        }
    }
}
