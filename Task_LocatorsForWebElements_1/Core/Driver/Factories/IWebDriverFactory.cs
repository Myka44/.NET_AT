using OpenQA.Selenium;

namespace CoreLayer.WebDriver.Factories
{
    public interface IWebDriverFactory
    {
        IWebDriver CreateDriver();
    }
}
