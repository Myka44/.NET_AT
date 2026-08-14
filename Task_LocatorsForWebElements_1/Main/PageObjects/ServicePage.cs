using OpenQA.Selenium;
using TestProject.PageObjects;

namespace BusinessLayer.PageObjects
{
    public class ServicePage : BasePage
    {
        private readonly By _relatedExpertiseHeaderLocator = By.XPath("//div[@class='section']//span[contains(normalize-space(.), 'Our Related Expertise')]");
        public ServicePage(CustomWebDriver driver) : base(driver)
        {
        }

        public bool IsRelatedExpertiseHeaderVisible()
        {
            try
            {
                CustomDriver.WaitUntilVisible(_relatedExpertiseHeaderLocator);
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

    }
}
