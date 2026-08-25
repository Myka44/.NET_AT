using BusinessLayer.PageObjects;
using Reqnroll;
using TestLayer.SessionContext;
using Xunit;

namespace BusinessActionLayer.StepDefinitions
{
    [Binding]
    public sealed class ServicesNavigationSteps
    {
        private readonly TestSessionContext _testSessionContext;
        private ServicePage _servicePage = null!;

        public ServicesNavigationSteps(TestSessionContext testSessionContext)
        {
            _testSessionContext = testSessionContext;
        }

        [When("the user hovers 'Services' button on the main navigation bar")]
        public void WhenTheUserHoversServicesButtonOnTheMainNavigationBar()
        {
            _testSessionContext.MainPage.HoverServicesNavBarButton();
        }

        [When("the user clicks {string} service category")]
        public void WhenTheUserClicksServiceCategory(string category)
        {
            _servicePage = _testSessionContext.MainPage.ClickServiceCategory(category);
        }

        [Then("the new page contains {string} in the title")]
        public void ThenTheNewPageContainsTextInTheTitle(string expectedText)
        {
            Assert.True(
                _servicePage.PageTitleContains(expectedText),
                $"Expected the page title to contain '{expectedText}'.");
        }

        [Then("the Our Related Expertise section is displayed on the page")]
        public void ThenTheRelatedExpertiseSectionIsDisplayed()
        {
            Assert.True(
                _servicePage.IsRelatedExpertiseHeaderVisible(),
                "The 'Our Related Expertise' section was not displayed.");
        }
    }
}
