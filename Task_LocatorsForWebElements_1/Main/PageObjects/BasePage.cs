namespace TestProject.PageObjects
{
    public abstract class BasePage
    {
        protected readonly CustomWebDriver Driver;

        protected BasePage(CustomWebDriver driver)
        {
            Driver = driver;
        }
    }
}
