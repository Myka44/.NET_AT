using OpenQA.Selenium;

namespace TestProject.PageObjects
{
    public class MainPage : BasePage
    {
        private readonly string _url;

        private readonly By _careersLocator = By.PartialLinkText("Careers");
        private readonly By _searchIconLocator = By.ClassName("search-icon");
        private readonly By _searchBarLocator = By.Id("new_form_search");
        private readonly By _findButtonLocator = By.CssSelector(".custom-search-button");
        private readonly By _searchResultItemLocator = By.ClassName("search-results__title-link");
        private readonly By _insightsMenuLocator = By.PartialLinkText("Insights");
        private readonly By _policyPdfLinkLocator = By.CssSelector("[href*='code-of-conduct/Code-Of-Conduct_01_26.pdf']");

        public MainPage(CustomWebDriver driver, string url) : base(driver)
        {
            _url = url;
        }

        public MainPage Open()
        {
            CustomDriver.NavigateTo(_url);
            return this;
        }

        public CareersPage GoToCareers()
        {
            CustomDriver.ClickWhenReady(_careersLocator);
            return new CareersPage(CustomDriver);
        }

        public MainPage OpenGlobalSearch()
        {
            CustomDriver.ClickWhenReady(_searchIconLocator);
            return this;
        }

        public MainPage EnterGlobalSearchKeyword(string keyword)
        {
            CustomDriver.TypeText(_searchBarLocator, keyword);
            return this;
        }

        public MainPage SubmitGlobalSearch()
        {
            CustomDriver.ClickWhenReady(_findButtonLocator);
            return this;
        }

        public List<string> GetGlobalSearchResultTitles() =>
            CustomDriver.WaitUntilAnyPresent(_searchResultItemLocator).Select(e => e.Text).ToList();


        public MainPage ClickCodeOfEthicalConductPdfLink()
        {
            CustomDriver.WaitUntil(d => d.FindElement(_policyPdfLinkLocator));
            CustomDriver.ScrollIntoViewCenter(_policyPdfLinkLocator);
            CustomDriver.ClickWhenReady(_policyPdfLinkLocator);
            return this;
        }

        public InsightsPage GoToInsights()
        {
            CustomDriver.ClickWhenReady(_insightsMenuLocator);
            return new InsightsPage(CustomDriver);
        }
    }
}
