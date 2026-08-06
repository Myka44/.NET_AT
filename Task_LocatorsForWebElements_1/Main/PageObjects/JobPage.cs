using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestProject.PageObjects
{
    public class JobPage : BasePage
    {
        private readonly By _searchByKeywordLocator = By.TagName("input");
        private readonly By _countryDropdownLocator = By.XPath("//div[@data-testid='country-dropdown']/descendant::div[contains(@class, 'dropdown__control')]");
        private readonly By _countryInputLocator = By.XPath("//div[@data-testid='country-dropdown']//input[contains(@class, 'dropdown__input')]");
        private readonly By _countryOptionLocator = By.XPath("//div[@data-testid='country-dropdown']//div[contains(@class, 'dropdown__option')]");
        private readonly By _countryDropDownValueLocator = By.CssSelector("div[data-testid='dropdown-value']");
        private readonly By _remoteFilterCheckboxLocator = By.CssSelector("label[for*='checkbox-vacancy_type-Remote']");
        private readonly By _searchButtonLocator = By.Name("submit_search_box_button");
        private readonly By _expandItemButtonLocator = By.CssSelector("span[data-testid='accordion-section-header-icon-container']");
        private readonly By _jobDescriptionLocator = By.CssSelector("div[data-testid='categories-container']");

        public JobPage(CustomWebDriver driver) : base(driver) { }

        public JobPage EnterSearchKeyword(string keyword)
        {
            CustomDriver.TypeText(_searchByKeywordLocator, keyword);
            return this;
        }

        public JobPage SubmitSearch()
        {
            CustomDriver.ClickAndWaitUntilUrlChanges(_searchButtonLocator);
            return this;
        }

        public JobPage SelectCountry(string country)
        {
            CustomDriver.WaitUntilClickable(_countryDropdownLocator);
            CustomDriver.ScrollIntoView(_countryDropdownLocator);
            CustomDriver.ClickSafe(_countryDropdownLocator);
            CustomDriver.TypeText(_countryInputLocator, country);
            CustomDriver.ClickSafeFromMultiple(_countryOptionLocator, o => o.Text.Equals(country));
            CustomDriver.WaitUntil(d =>
            {
                var value = d.FindElement(_countryDropDownValueLocator);
                return value.Text.Equals(country) && value.Displayed && value.Enabled;
            });

            return this;
        }

        public JobPage ToggleRemoteFilter()
        {
            CustomDriver.ClickSafe(_remoteFilterCheckboxLocator);
            return this;
        }

        public JobPage ExpandLastResult()
        {
            CustomDriver.ClickSafeLast(_expandItemButtonLocator);
            return this;
        }

        public string GetJobDescriptionText()
        {
            return CustomDriver.WaitUntilVisibleLast(_jobDescriptionLocator).Text;
        }
    }
}
