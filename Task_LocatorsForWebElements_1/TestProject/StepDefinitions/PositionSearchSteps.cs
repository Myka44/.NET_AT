using Reqnroll;
using TestLayer.SessionContext;
using TestProject.PageObjects;

namespace TestLayer.StepDefinitions
{
    [Binding]
    public sealed class PositionSearchSteps
    {
        private readonly TestSessionContext _testSessionContext;
        private JobPage _jobPage = null!;
        private string _latestPositionDescription = string.Empty;

        public PositionSearchSteps(TestSessionContext testSessionContext)
        {
            _testSessionContext = testSessionContext;
        }

        [When("the user opens Careers and starts a position search")]
        public void WhenTheUserOpensCareersAndStartsAPositionSearch()
        {
            _jobPage = _testSessionContext.MainPage
                .GoToCareers()
                .StartYourSearch();
        }

        [When("enters {string} as the role or keyword")]
        public void WhenTheUserEntersTheRoleOrKeyword(string programmingLanguage)
        {
            _jobPage.EnterSearchKeyword(programmingLanguage);
        }

        [When("submits the position search")]
        public void WhenTheUserSubmitsThePositionSearch()
        {
            _jobPage.SubmitSearch();
        }

        [When("filters positions by {string} and Remote")]
        public void WhenTheUserFiltersPositionsByCountryAndRemote(string country)
        {
            _jobPage
                .SelectCountry(country)
                .ToggleRemoteFilter();
        }

        [When("expands the latest position result")]
        public void WhenTheUserExpandsTheLatestPositionResult()
        {
            _latestPositionDescription = _jobPage
                .ExpandLastResult()
                .GetJobDescriptionText();
        }

        [Then("the latest position description contains {string}")]
        public void ThenTheLatestPositionDescriptionContains(string programmingLanguage)
        {
            Assert.Contains(
                programmingLanguage,
                _latestPositionDescription,
                StringComparison.OrdinalIgnoreCase);
        }
    }
}
