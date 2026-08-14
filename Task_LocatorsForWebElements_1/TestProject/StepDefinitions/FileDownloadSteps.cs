using Reqnroll;
using TestLayer.SessionContext;
using TestLayer.Utils;

namespace TestLayer.StepDefinitions
{
    [Binding]
    public sealed class FileDownloadSteps
    {
        private readonly TestSessionContext _testSessionContext;

        public FileDownloadSteps(TestSessionContext testSessionContext)
        {
            _testSessionContext = testSessionContext;
        }

        [When("the user downloads the Code of Ethical Conduct PDF from the footer")]
        public void WhenTheUserDownloadsTheCodeOfEthicalConductPdfFromTheFooter()
        {
            _testSessionContext.MainPage.ClickCodeOfEthicalConductPdfLink();
        }

        [Then("the file {string} is downloaded")]
        public void ThenTheFileIsDownloaded(string expectedFileName)
        {
            string expectedFilePath = Path.Combine(
                _testSessionContext.DownloadDirectory,
                expectedFileName);

            bool downloaded = DownloadUtils.WaitForFileToBeDownloaded(
                expectedFilePath,
                TimeSpan.FromSeconds(30));

            Assert.True(
                downloaded,
                $"Expected file '{expectedFileName}' was not found in '{_testSessionContext.DownloadDirectory}'.");
        }
    }
}
