namespace TestProject.PageObjects
{
    public abstract class BasePage
    {
        protected readonly CustomWebDriver CustomDriver;

        protected BasePage(CustomWebDriver driver)
        {
            CustomDriver = driver;
        }
    }
}
