using log4net;

namespace TestProject.PageObjects
{
    public abstract class BasePage
    {
        protected readonly CustomWebDriver CustomDriver;
        protected ILog Log => LogManager.GetLogger(GetType());

        protected BasePage(CustomWebDriver driver)
        {
            CustomDriver = driver;
        }
    }
}
