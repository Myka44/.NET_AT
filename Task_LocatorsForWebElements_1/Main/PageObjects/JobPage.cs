using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestProject.PageObjects
{
    public class JobPage : BasePage
    {
        private readonly By _searchByKeywordLocator = By.TagName("input");
        private readonly By _countryDropdownLocator = By.CssSelector("[data-testid='country-dropdown'] .dropdown__control");
        private readonly By _countryInputLocator = By.CssSelector("input.dropdown__input");
        private readonly By _countryOptionLocator = By.CssSelector("div[class*='dropdown__option']");
        private readonly By _countryDropDownMenuLocator = By.ClassName("dropdown__menu");
        private readonly By _countryDropDownValueLocator = By.CssSelector("div[data-testid='dropdown-value']");
        private readonly By _remoteFilterCheckboxLocator = By.CssSelector("label[for*='checkbox-vacancy_type-Remote']");
        private readonly By _searchButtonLocator = By.XPath("//button[@name='submit_search_box_button']");
        private readonly By _expandItemButtonLocator = By.CssSelector("span[data-testid='accordion-section-header-icon-container']");
        private readonly By _jobDescriptionLocator = By.CssSelector("div[data-testid='categories-container']");

        public JobPage(CustomWebDriver driver) : base(driver) { }

        public JobPage EnterSearchKeyword(string keyword)
        {
            Driver.TypeText(_searchByKeywordLocator, keyword);
            return this;
        }

        public JobPage SubmitSearch()
        {
            Driver.ClickWhenReady(_searchButtonLocator);
            return this;
        }

        public JobPage SelectCountry(string country)
        {
            var dropdown = Driver.WaitUntilClickable(_countryDropdownLocator);
            Driver.ScrollIntoView(dropdown);

            Driver.ClickSafe(_countryDropdownLocator);

            Driver.WaitUntilClickable(_countryDropDownMenuLocator);

            Thread.Sleep(500);

            Driver.TypeText(_countryInputLocator, country);

            Driver.ClickSafeFromMultiple(_countryOptionLocator, o => o.Text.Equals(country));

            Driver.WaitUntil(d =>
            {
                var value = d.FindElement(_countryDropDownValueLocator);
                return value.Text.Equals(country) && value.Displayed && value.Enabled;
            });

            return this;
        }

        public JobPage ToggleRemoteFilter()
        {
            Driver.ClickSafe(_remoteFilterCheckboxLocator);
            return this;
        }

        public JobPage ExpandFirstResult()
        {
            Driver.ClickSafe(_expandItemButtonLocator);
            return this;
        }

        public string GetJobDescriptionText() => Driver.GetText(_jobDescriptionLocator);
    }
}
