using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

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
            Log.Info($"Opening main page: {_url}");
            CustomDriver.NavigateTo(_url);
            CustomDriver.AcceptCookiesIfPresent();
            return this;
        }

        public CareersPage GoToCareers()
        {
            Log.Info("Opening Careers page");
            CustomDriver.ClickWhenReady(_careersLocator);
            return new CareersPage(CustomDriver);
        }

        public MainPage OpenGlobalSearch()
        {
            Log.Info("Opening global search");
            CustomDriver.ClickWhenReady(_searchIconLocator);
            return this;
        }

        public MainPage EnterGlobalSearchKeyword(string keyword)
        {
            Log.Info($"Entering global search keyword: {keyword}");
            CustomDriver.TypeText(_searchBarLocator, keyword);
            return this;
        }

        public MainPage SubmitGlobalSearch()
        {
            Log.Info("Submitting global search");
            CustomDriver.ClickWhenReady(_findButtonLocator);
            return this;
        }

        public List<string> GetGlobalSearchResultTitles()
        {
            Log.Info("Reading global search result titles");

            try
            {
                return CustomDriver.WaitUntilAnyPresent(_searchResultItemLocator).Select(e => e.Text).ToList();
            }
            catch (WebDriverTimeoutException)
            {
                Log.Warn("No global search result titles were found before timeout");
                return new List<string>();
            }
        }

        public MainPage ClickCodeOfEthicalConductPdfLink()
        {
            Log.Info("Clicking Code of Ethical Conduct PDF link");
            CustomDriver.WaitUntil(d => d.FindElement(_policyPdfLinkLocator));
            CustomDriver.ScrollIntoViewCenter(_policyPdfLinkLocator);
            CustomDriver.ClickWhenReady(_policyPdfLinkLocator);
            return this;
        }

        public InsightsPage GoToInsights()
        {
            Log.Info("Opening Insights page");
            CustomDriver.ClickWhenReady(_insightsMenuLocator);
            return new InsightsPage(CustomDriver);
        }
    }
}
