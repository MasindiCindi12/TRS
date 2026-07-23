using TRS.Web.Automation.Assertions;
using TRS.Web.Automation.Configuration;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Pages;

namespace TRS.Web.Automation.Tests
{
    [TestFixture]
    public class LoginTests : BaseTest
    {
        private LoginPage _loginPage = null!;
        private AppSettings _settings = null!;
        private DashboardPage _dashboardPage = null!;

        [SetUp]
        public void SetUpLoginPage()
        {
            _settings = AppSettingsProvider.Current;
            _loginPage = new LoginPage(Driver, ExtentTest, Recorder);
            _dashboardPage = new DashboardPage(Driver, ExtentTest, Recorder);

            ExtentTest.Info(PreConditionBanner);
            _loginPage.NavigateTo(_settings.BaseUrl, _settings.LoginPath);

        }

        [Test]
        [Category("Login Tests")]
        public async Task Login_WithValidCredentials_ShouldReachDashboard()
        {
            Assert.That(_settings.LoginEmail, Is.Not.Empty,
                "Set LoginEmail in Configuration/appsettings.local.json before running this test.");
            Assert.That(_settings.LoginPassword, Is.Not.Empty,
                "Set LoginPassword in Configuration/appsettings.local.json before running this test.");

            var result = await _loginPage.SubmitLoginAsync(
                _settings.LoginEmail,
                _settings.LoginPassword,
                _settings.DashboardPath,
                responseTimeout: TimeSpan.FromSeconds(15),
                redirectTimeout: TimeSpan.FromSeconds(20));

            ExtentTest.Info("Expected: Login succeeds (200 OK) and redirects to the dashboard.");
            ExtentTest.Info($"Actual: Network status {result.StatusCode?.ToString() ?? "no response captured"}, final URL: {result.FinalUrl}.");
            LoginAssertions.AssertLoginSucceeded(result, _settings.DashboardPath);
        }

        [Test]
        [Category("Login Tests")]
        public async Task Login_WithInvalidCredentials_ShouldBeRejected()
        {
            var result = await _loginPage.SubmitLoginAsync(
                "invalid.user@example.com",
                "IncorrectPassword123",
                _settings.DashboardPath,
                responseTimeout: TimeSpan.FromSeconds(15),
                redirectTimeout: TimeSpan.FromSeconds(5));

            ExtentTest.Info("Expected: Login is rejected (401 Unauthorized) and the user remains on the Sign In page.");
            ExtentTest.Info($"Actual: Network status {result.StatusCode?.ToString() ?? "no response captured"}, final URL: {result.FinalUrl}.");
            LoginAssertions.AssertLoginFailed(result, _settings.LoginPath);
        }

        [Test]
        [Category("Login Tests")]
        public async Task Logout_WhenClicked_ShouldReturnToSignIn()
        {
            Assert.That(_settings.LoginEmail, Is.Not.Empty,
                "Set LoginEmail in Configuration/appsettings.local.json before running this test.");
            Assert.That(_settings.LoginPassword, Is.Not.Empty,
                "Set LoginPassword in Configuration/appsettings.local.json before running this test.");

            var loginResult = await _loginPage.SubmitLoginAsync(
                _settings.LoginEmail,
                _settings.LoginPassword,
                _settings.DashboardPath,
                responseTimeout: TimeSpan.FromSeconds(15),
                redirectTimeout: TimeSpan.FromSeconds(20));
            LoginAssertions.AssertLoginSucceeded(loginResult, _settings.DashboardPath);

            var result = _dashboardPage.SubmitLogOut(_settings.LoginPath, redirectTimeout: TimeSpan.FromSeconds(15));

            ExtentTest.Info("Expected: Logging out redirects back to the Sign In page.");
            ExtentTest.Info($"Actual: Final URL: {result.FinalUrl}.");
            LogoutAssertions.AssertLogoutSucceeded(result, _settings.LoginPath);
        }
    }
}
