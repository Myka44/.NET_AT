using OpenQA.Selenium;

namespace TestProject.PageObjects
{
    public class InsightsPage : BasePage
    {
        private readonly By _activeSlideLocator = By.CssSelector(".owl-item.active");
        private readonly By _activeSlideTitleLocator = By.CssSelector(".owl-item.active .text-ui-23");
        private readonly By _activeSlideReadMoreLocator = By.CssSelector(".owl-item.active .slider-cta-link");
        private readonly By _nextArrowLocator = By.CssSelector(".slider__right-arrow");

        public InsightsPage(CustomWebDriver driver) : base(driver) { }

        public InsightsPage SwipeCarousel(int times)
        {
            for (int i = 0; i < times; i++)
            {
                Driver.ClickWhenReady(_nextArrowLocator);
                Driver.WaitUntilVisible(_activeSlideLocator);
                Thread.Sleep(500);
            }

            return this;
        }

        public string GetCurrentArticleTitle() => Driver.GetText(_activeSlideTitleLocator);

        public ArticlePage ClickReadMore()
        {
            Driver.ClickWhenReady(_activeSlideReadMoreLocator);
            return new ArticlePage(Driver);
        }
    }
}