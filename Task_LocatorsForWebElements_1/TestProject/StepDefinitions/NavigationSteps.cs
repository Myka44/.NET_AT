using Reqnroll;
using TestLayer.SessionContext;
using TestProject.PageObjects;

namespace TestLayer.StepDefinitions
{
    [Binding]
    public sealed class NavigationSteps
    {
        private readonly TestSessionContext _testSessionContext;

        public NavigationSteps(TestSessionContext testSessionContext)
        {
            _testSessionContext = testSessionContext;
        }

        [Given("the user is on the EPAM home page")]
        public void GivenTheUserIsOnTheEpamHomePage()
        {
            _testSessionContext.MainPage = new MainPage(
                _testSessionContext.Driver,
                _testSessionContext.Settings.Environment.BaseUrl);

            _testSessionContext.MainPage.Open();
        }
    }
}
