using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.PageObjects
{
    internal class temp
    {
        //used LinkText locator, because the link text only contains the word "Careers" and it doesn't have unique classes
        private readonly By _careersLocator = By.LinkText("Careers");
        //used ClassName locator, because the element contains a unique class in the page and it doesn't have an id
        private readonly By _searchIconLocator = By.ClassName("search-icon");
        //used Id locator, because the element contains a stable id atribute
        private readonly By _searchBarLocator = By.Id("new_form_search");
        //used a CssSelector locator, because the element contains multiple classes and I need to locate the element by the most stable class
        private readonly By _findButtonLocator = By.CssSelector(".custom-search-button");
        //used to find all the relevant result items that contain the search option parameter
        //private readonly By _resultItemLocator = By.PartialLinkText(searchOption);
        //used a CssSelector locator, because the element contains a link to the jobs page and it seems to be most stable
        private readonly By _startYourSearchLocator = By.CssSelector("a.button-body[href*='careers.epam.com/en/jobs']");

        //used a TagName locator, because the relevant input element is closest to the top of the DOM
        private readonly By _searchByKeywordLocator = By.TagName("input");

        //used a XPath locator with axis, because the element doesn't contain unique classes in the page and is nested by a parent div element with data-testid='country-dropdown' attribute
        private readonly By _countryDropdownLocator = By.XPath("//div[@data-testid='country-dropdown']/descendant::div[contains(@class, 'dropdown__control')]");
        //used a XPath locator with axis, because the element is nested by a parent div element with data-testid='country-dropdown'
        private readonly By _countryInputLocator = By.XPath("//div[@data-testid='country-dropdown']//input[contains(@class, 'dropdown__input')]");
        //used a XPath locator with axis, because the element is nested by a parent div element with data-testid='country-dropdown'
        private readonly By _countryOptionLocator = By.XPath("//div[@data-testid='country-dropdown']//div[contains(@class, 'dropdown__option')]");
        //used a XPath locator with axis, because the element is nested by a parent div element with data-testid='country-dropdown'
        private readonly By _countryDropDownMenu = By.XPath("//div[@data-testid='country-dropdown']//div[contains(@class, 'dropdown__menu')]");
        //used a XPath locator with axis, because the element is nested by a parent div element with data-testid='country-dropdown'
        private readonly By _countryDropDownValue = By.CssSelector("div[data-testid='dropdown-value']");
        //used a CssSelector locator, because the element contains a button with for attribute which is linked to the relevant button id
        private readonly By _remoteFilterCheckboxLocator = By.CssSelector("label[for*='checkbox-vacancy_type-Remote']");
        //used a Name locator, because the element contains a button with unique name
        private readonly By _searchButtonLocator = By.Name("submit_search_box_button");
        //used a CssSelector locator, because the element contains a stable data-testid attribute
        private readonly By _expandItemButtonLocator = By.CssSelector("span[data-testid='accordion-section-header-icon-container']");
        //used a CssSelector locator, because the element contains a stable data-testid attribute
        private readonly By _jobDescriptionLocator = By.CssSelector("div[data-testid='categories-container']");
    }
}
