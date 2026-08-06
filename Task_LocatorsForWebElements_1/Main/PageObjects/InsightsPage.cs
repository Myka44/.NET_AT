using OpenQA.Selenium;

namespace TestProject.PageObjects
{
    public class InsightsPage : BasePage
    {
        private readonly string _sliderSelector = ".owl-item.active";
        private readonly By _activeSlideLocator;
        private readonly By _activeSlideTitleLocator = By.CssSelector(".owl-item.active .text-ui-23");
        private readonly By _activeSlideReadMoreLocator = By.CssSelector(".owl-item.active .slider-cta-link");
        private readonly By _nextArrowLocator = By.CssSelector(".slider__right-arrow");

        public InsightsPage(CustomWebDriver driver) : base(driver)
        {
            _activeSlideLocator = By.CssSelector(_sliderSelector);
        }

        public InsightsPage SwipeCarousel(int times)
        {
            Log.Info($"Swiping carousel {times} times");

            ((IJavaScriptExecutor)CustomDriver.Driver).ExecuteScript(@$"
             jQuery('{_sliderSelector}').trigger('stop.owl.autoplay');
            ");

            for (int i = 0; i < times; i++)
            {
                CustomDriver.ClickWhenReady(_nextArrowLocator);
                CustomDriver.WaitUntilVisible(_activeSlideLocator);
                Thread.Sleep(1000);
            }

            return this;
        }

        public string GetCurrentArticleTitle()
        {
            Log.Info("Reading current carousel article title");
            return CustomDriver.GetText(_activeSlideTitleLocator);
        }

        public ArticlePage ClickReadMore()
        {
            Log.Info("Opening active carousel article");
            CustomDriver.ClickWhenReady(_activeSlideReadMoreLocator);
            return new ArticlePage(CustomDriver);
        }
    }
}
