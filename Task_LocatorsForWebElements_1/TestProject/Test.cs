using TestProject.PageObjects;

namespace TestProject
{
    public class Test : BaseTest
    {
        public Test()
        {
            Log.Info("Starting tests");
        }

        [Theory]
        [InlineData("JavaScript", "Republic of Lithuania")]
        [InlineData("Java", "Poland")]
        public void Task1_PositionSearchResultDescriptionContainsSearchKeyword(string searchKeyword, string searchCountry)
        {
            ExecuteTest(() =>
            {
                Log.Info("Hello World of Logging :) ...");

                var mainPage = new MainPage(_driver, Settings.Environment.BaseUrl);
                mainPage.Open();

                var jobPage = mainPage.GoToCareers().StartYourSearch();

                string descriptionText = jobPage
                    .EnterSearchKeyword(searchKeyword)
                    .SubmitSearch()
                    .SelectCountry(searchCountry)
                    .ToggleRemoteFilter()
                    .ExpandLastResult()
                    .GetJobDescriptionText();

                Assert.True(
                    descriptionText.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase),
                    $"expected description did not contain '{searchKeyword}'");
            });
        }

        [Theory]
        [InlineData("BLOCKCHAIN")]
        [InlineData("Cloud")]
        [InlineData("Automation")]
        public void Task2_GlobalSearchWithValidInputResultsContainSearchKeyword(string searchKeyword)
        {
            ExecuteTest(() =>
            {
                var mainPage = new MainPage(_driver, Settings.Environment.BaseUrl);
                mainPage.Open();

                List<string> resultTitles = mainPage
                    .OpenGlobalSearch()
                    .EnterGlobalSearchKeyword(searchKeyword)
                    .SubmitGlobalSearch()
                    .GetGlobalSearchResultTitles();

                resultTitles.ForEach(title => Log.Info(title));

                Assert.True(resultTitles.Count > 0, $"expected at least one search result for '{searchKeyword}'");
                Assert.True(resultTitles.All(title => title.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase)), $"all search results did not contain '{searchKeyword}'");
            });
        }

        [Theory]
        [InlineData("Code-Of-Conduct_01_26.pdf")]
        public void Task3_CodeOfEthicalConductPdfDownloadsSuccessfully(string expectedFileName)
        {
            ExecuteTest(() =>
            {
                var mainPage = new MainPage(_driver, Settings.Environment.BaseUrl);
                mainPage.Open();
                mainPage.ClickCodeOfEthicalConductPdfLink();

                string expectedFilePath = Path.Combine(DownloadDirectory, expectedFileName);
                bool downloaded = WaitForFileToBeDownloaded(expectedFilePath, TimeSpan.FromSeconds(30));

                Assert.True(downloaded, $"expected file '{expectedFileName}' was not found in '{DownloadDirectory}'.");
            });
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void Task4_ArticleTitleMatchesCarouselTitle(int swipeCount)
        {
            ExecuteTest(() =>
            {
                var mainPage = new MainPage(_driver, Settings.Environment.BaseUrl);
                mainPage.Open();

                var insightsPage = mainPage.GoToInsights();
                insightsPage.SwipeCarousel(swipeCount);

                string carouselArticleTitle = insightsPage.GetCurrentArticleTitle();
                Log.Info($"carousel article title: {carouselArticleTitle}");

                var articlePage = insightsPage.ClickReadMore();
                string articlePageTitle = articlePage.GetArticleTitle();
                Log.Info($"article page title: {articlePageTitle}");

                Assert.Equal(carouselArticleTitle, articlePageTitle, ignoreCase: true);
                
            });
        }

        private static bool WaitForFileToBeDownloaded(string filePath, TimeSpan timeout)
        {
            DateTime deadline = DateTime.UtcNow.Add(timeout);
            while (DateTime.UtcNow < deadline)
            {
                if (File.Exists(filePath))
                {
                    return true;
                }

                Thread.Sleep(500);
            }
            return false;
        }
    }
}
