using Reqnroll;
using TestLayer.SessionContext;

namespace TestLayer.StepDefinitions
{
    [Binding]
    public sealed class GlobalSearchSteps
    {
        private readonly TestSessionContext _testSessionContext;
        private List<string> _resultTitles = new();

        public GlobalSearchSteps(TestSessionContext testSessionContext)
        {
            _testSessionContext = testSessionContext;
        }

        [When("the user performs a global search for {string}")]
        public void WhenTheUserPerformsAGlobalSearchFor(string searchKeyword)
        {
            _resultTitles = _testSessionContext.MainPage
                .OpenGlobalSearch()
                .EnterGlobalSearchKeyword(searchKeyword)
                .SubmitGlobalSearch()
                .GetGlobalSearchResultTitles();
        }

        [Then("at least one global search result is displayed")]
        public void ThenAtLeastOneGlobalSearchResultIsDisplayed()
        {
            Assert.NotEmpty(_resultTitles);
        }

        [Then("all global search result titles contain {string}")]
        public void ThenAllGlobalSearchResultTitlesContain(string searchKeyword)
        {
            bool allTitlesContainKeyword = _resultTitles.All(title =>
                title.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase));

            Assert.True(
                allTitlesContainKeyword,
                $"Expected every global search result title to contain '{searchKeyword}'.");
        }
    }
}
