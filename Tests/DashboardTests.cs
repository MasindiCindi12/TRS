using TRS.Web.Automation.Assertions;
using TRS.Web.Automation.Configuration;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Pages;
using TRS.Web.Automation.TestData;

namespace TRS.Web.Automation.Tests
{
    [TestFixture]
    public class DashboardTests : BaseTest
    {
        private DashboardPage _dashboardPage = null!;
        private HobbiesPage _hobbiesPage = null!;
        private AppSettings _settings = null!;

        [SetUp]
        public async Task SetUpLoggedInSession()
        {
            _settings = AppSettingsProvider.Current;

            AssertCredentialsConfigured(_settings);

            var loginPage = new LoginPage(Driver, ExtentTest, Recorder);
            _dashboardPage = new DashboardPage(Driver, ExtentTest, Recorder);
            _hobbiesPage = new HobbiesPage(Driver, ExtentTest, Recorder);

            ExtentTest.Info(PreConditionBanner);
            loginPage.NavigateTo(_settings.BaseUrl, _settings.LoginPath);

            var loginResult = await loginPage.SubmitLoginAsync(
                _settings.LoginEmail,
                _settings.LoginPassword,
                _settings.DashboardPath,
                responseTimeout: TimeSpan.FromSeconds(15),
                redirectTimeout: TimeSpan.FromSeconds(20));

            LoginAssertions.AssertLoginSucceeded(ExtentTest, loginResult, _settings.DashboardPath);

            ExtentTest.Info(EndPreConditionBanner);
        }

        [Test]
        [Category("Dashboard")]
        public void HobbyDistributionChart_AfterAddingHobby_ShouldReflectNewCount()
        {
            _dashboardPage.NavigateTo(_settings.BaseUrl, _settings.DashboardPath);
            var countBefore = _dashboardPage.GetHobbyDistributionCount(TestDataFactory.HobbyTypes.Music);

            var hobbyName = TestDataFactory.UniqueName(TestDataFactory.HobbyNames.Default);
            _hobbiesPage.NavigateTo(_settings.BaseUrl, _settings.HobbiesPath);
            var addResult = _hobbiesPage.SubmitAddHobby(hobbyName, TestDataFactory.HobbyTypes.Music);
            HobbyAssertions.AssertHobbyAdded(ExtentTest, addResult);

            _dashboardPage.NavigateTo(_settings.BaseUrl, _settings.DashboardPath);
            var countAfter = _dashboardPage.GetHobbyDistributionCount(TestDataFactory.HobbyTypes.Music);

            DashboardAssertions.AssertHobbyDistributionCountIncreased(ExtentTest, TestDataFactory.HobbyTypes.Music, countBefore, countAfter);
        }
    }
}
