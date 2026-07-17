using OpenQA.Selenium;

namespace TestProject.PageObjects
{
    public class ArticlePage : BasePage
    {
        private readonly By _articleTitleLocator = By.TagName("h1");

        public ArticlePage(CustomWebDriver driver) : base(driver) { }

        public string GetArticleTitle() => Driver.GetText(_articleTitleLocator);
    }
}
