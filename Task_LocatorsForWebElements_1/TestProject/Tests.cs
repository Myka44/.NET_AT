using Microsoft.Extensions.Configuration;
using OpenQA.Selenium.Chrome;
using TestProject.PageObjects;
using Xunit.Abstractions;

namespace TestProject
{
    public class Tests : IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly CustomWebDriver _driver;
        private readonly string _downloadDirectory;

        private static readonly string MainPageUrl = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build()["MainPageUrl"]!;

        public Tests(ITestOutputHelper output)
        {
            _output = output;

            _downloadDirectory = Path.Combine(Path.GetTempPath(), "test_downloads" + Guid.NewGuid());
            Directory.CreateDirectory(_downloadDirectory);

            var options = new ChromeOptions();

            options.AddUserProfilePreference("download.default_directory", _downloadDirectory);
            options.AddUserProfilePreference("download.prompt_for_download", false);

            var chromeDriver = new ChromeDriver(options);
            chromeDriver.Manage().Window.Maximize();

            _driver = new CustomWebDriver(chromeDriver, TimeSpan.FromSeconds(15));
        }

        public void Dispose()
        {
            _driver.Quit();

            if (Directory.Exists(_downloadDirectory))
            {
                Directory.Delete(_downloadDirectory, true);
            }
        }

        [Theory]
        [InlineData("JavaScript", "Republic of Lithuania")]
        [InlineData("Java", "Poland")]
        public void Task1_PositionSearchResultDescriptionContainsSearchKeyword(string searchKeyword, string searchCountry)
        {
            var mainPage = new MainPage(_driver, MainPageUrl);
            mainPage.Open();

            var jobPage = mainPage.GoToCareers().StartYourSearch();

            string descriptionText = jobPage
                .EnterSearchKeyword(searchKeyword)
                .SubmitSearch()
                .SelectCountry(searchCountry)
                .ToggleRemoteFilter()
                .ExpandLastResult()
                .GetJobDescriptionText();

            _output.WriteLine($"description text: {descriptionText}");

            Assert.True(
                descriptionText.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase),
                $"Expected description to contain '{searchKeyword}' but it did not.");
        }

        [Theory]
        [InlineData("BLOCKCHAIN")]
        [InlineData("Cloud")]
        [InlineData("Automation")]
        public void Task2_GlobalSearchWithValidInputResultsContainSearchKeyword(string searchKeyword)
        {
            var mainPage = new MainPage(_driver, MainPageUrl);
            mainPage.Open();

            List<string> resultTitles = mainPage
                .OpenGlobalSearch()
                .EnterGlobalSearchKeyword(searchKeyword)
                .SubmitGlobalSearch()
                .GetGlobalSearchResultTitles();

            resultTitles.ForEach(title => _output.WriteLine(title));

            Assert.True(resultTitles.Count > 0, $"Expected at least one search result for '{searchKeyword}' but found none.");
            Assert.True(resultTitles.All(title => title.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase)));
        }

        [Theory]
        [InlineData("Code-Of-Conduct_01_26.pdf")]
        public void Task3_CodeOfEthicalConductPdfDownloadsSuccessfully(string expectedFileName)
        {
            var mainPage = new MainPage(_driver, MainPageUrl);
            mainPage.Open();
            mainPage.ClickCodeOfEthicalConductPdfLink();

            string expectedFilePath = Path.Combine(_downloadDirectory, expectedFileName);
            bool downloaded = WaitForFileToBeDownloaded(expectedFilePath, TimeSpan.FromSeconds(30));

            Assert.True(downloaded, $"expected file '{expectedFileName}' was not found in '{_downloadDirectory}'.");
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void Task4_ArticleTitleMatchesCarouselTitle(int swipeCount)
        {
            var mainPage = new MainPage(_driver, MainPageUrl);
            mainPage.Open();

            var insightsPage = mainPage.GoToInsights();
            insightsPage.SwipeCarousel(swipeCount);

            string carouselArticleTitle = insightsPage.GetCurrentArticleTitle();
            _output.WriteLine($"carousel article title: {carouselArticleTitle}");

            var articlePage = insightsPage.ClickReadMore();
            string articlePageTitle = articlePage.GetArticleTitle();
            _output.WriteLine($"article page title: {articlePageTitle}");

            Assert.Equal(carouselArticleTitle, articlePageTitle, ignoreCase: true);
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