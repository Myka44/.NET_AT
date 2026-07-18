using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestProject.PageObjects
{
    public class CustomWebDriver
    {
        public IWebDriver Driver { get; }

        private readonly WebDriverWait _wait;
        private readonly WebDriverWait _ignoreStaleWait;

        public CustomWebDriver(IWebDriver driver, TimeSpan timeout)
        {
            Driver = driver;
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            _wait = new WebDriverWait(Driver, timeout);

            _ignoreStaleWait = new WebDriverWait(Driver, timeout);
            _ignoreStaleWait.IgnoreExceptionTypes(
                typeof(StaleElementReferenceException),
                typeof(ElementClickInterceptedException)
                );
        }

        public void NavigateTo(string url) => Driver.Navigate().GoToUrl(url);

        public void WriteSafe(By locator, string text)
        {
            WaitAction(locator, _ignoreStaleWait, x => x.SendKeys(text));
        }

        public void ClickSafe(By locator)
        {
            WaitAction(locator, _ignoreStaleWait, x => x.Click());
        }

        public void ClickSafeFromMultiple(By locator, Func<IWebElement, bool> elementFilter)
        {
            WaitActionMultiple(locator, _ignoreStaleWait, x => x.Click(), elementFilter);
        }

        private void WaitAction(By locator, WebDriverWait wait, Action<IWebElement> action)
        {
            wait.Until(d =>
            {
                IWebElement element = d.FindElement(locator);

                if (element.Displayed && element.Enabled)
                {
                    action(element);
                    return true;
                }
                return false;
            });
        }

        private void WaitActionMultiple(By locator, WebDriverWait wait, Action<IWebElement> action, Func<IWebElement, bool> elementFilter)
        {
            wait.Until(d =>
            {
                IWebElement element = d.FindElements(locator).FirstOrDefault(elementFilter);

                if (element != null && element.Displayed && element.Enabled)
                {
                    action(element);
                    return true;
                }
                return false;
            });
        }

        public IWebElement WaitUntilVisible(By locator) =>
            _wait.Until(d =>
            {
                var element = d.FindElement(locator);
                return element.Displayed ? element : null;
            });


        public IWebElement WaitUntilClickable(By locator) =>
            _ignoreStaleWait.Until(d =>
            {
                var element = d.FindElement(locator);
                return (element.Displayed && element.Enabled) ? element : null;
            });

        public IReadOnlyCollection<IWebElement> WaitUntilAnyPresent(By locator) =>
            _wait.Until(d =>
            {
                var elements = d.FindElements(locator);
                return elements.Count > 0 ? elements : null;
            });

        public void ClickWhenReady(By locator) => WaitUntilClickable(locator).Click();

        public void TypeText(By locator, string text)
        {
            var element = WaitUntilClickable(locator);
            element.SendKeys(text);
        }

        public string GetText(By locator) => WaitUntilVisible(locator).Text;

        public void ScrollIntoView(IWebElement element) =>
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);

        public void ScrollIntoView(By locator) => ScrollIntoView(WaitUntilVisible(locator));


        public void ScrollIntoViewCenter(IWebElement element) =>
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", element);

        public void ScrollIntoViewCenter(By locator) => ScrollIntoViewCenter(WaitUntilVisible(locator));

        public void WaitUntil(Func<IWebDriver, bool> condition) => _wait.Until(condition);

        public T WaitUntil<T>(Func<IWebDriver, T> condition) => _wait.Until(condition);

        public T WaitIgnoringStaleness<T>(Func<IWebDriver, T> condition) => _ignoreStaleWait.Until(condition);

        public void Quit() => Driver.Quit();
    }
}
