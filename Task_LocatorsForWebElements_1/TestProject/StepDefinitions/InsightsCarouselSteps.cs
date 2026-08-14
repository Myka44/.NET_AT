using Reqnroll;
using TestLayer.SessionContext;
using TestProject.PageObjects;

namespace TestLayer.StepDefinitions
{
    [Binding]
    public sealed class InsightsCarouselSteps
    {
        private readonly TestSessionContext _testSessionContext;
        private InsightsPage _insightsPage = null!;
        private string _carouselArticleTitle = string.Empty;
        private string _articlePageTitle = string.Empty;

        public InsightsCarouselSteps(TestSessionContext testSessionContext)
        {
            _testSessionContext = testSessionContext;
        }

        [When("the user opens Insights and swipes the carousel {int} times")]
        public void WhenTheUserOpensInsightsAndSwipesTheCarousel(int swipeCount)
        {
            _insightsPage = _testSessionContext.MainPage
                .GoToInsights()
                .SwipeCarousel(swipeCount);
        }

        [When("notes the active article title and opens the article")]
        public void WhenTheUserNotesTheActiveArticleTitleAndOpensTheArticle()
        {
            _carouselArticleTitle = _insightsPage.GetCurrentArticleTitle();
            _articlePageTitle = _insightsPage
                .ClickReadMore()
                .GetArticleTitle();
        }

        [Then("the article page title matches the selected carousel title")]
        public void ThenTheArticlePageTitleMatchesTheSelectedCarouselTitle()
        {
            Assert.Equal(
                _carouselArticleTitle,
                _articlePageTitle,
                ignoreCase: true);
        }
    }
}
