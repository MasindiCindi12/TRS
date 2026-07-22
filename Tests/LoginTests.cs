using TRS.Web.Automation.Assertions;
using TRS.Web.Automation.Configuration;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Pages;

namespace TRS.Web.Automation.Tests
{
    [TestFixture]
    public class LoginTests : BaseTest
    {
        private const string PreConditionBanner =
            "====================================Pre-Condition====================================";

        private LoginPage _loginPage = null!;
        private AppSettings _settings = null!;

        [SetUp]
        public void SetUpLoginPage()
        {
            _settings = AppSettingsProvider.Current;
            _loginPage = new LoginPage(Driver, ExtentTest, Recorder);

            ExtentTest.Info(PreConditionBanner);
            _loginPage.NavigateTo(_settings.BaseUrl, _settings.LoginPath);
        }

        [Test]
        public async Task Login_WithValidCredentials_ReachesDashboard()
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

            ExtentTest.Info($"Network: {result.StatusCode?.ToString() ?? "no response captured"}");
            LoginAssertions.AssertLoginSucceeded(result, _settings.DashboardPath);
        }

        [Test]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            var result = await _loginPage.SubmitLoginAsync(
                "invalid.user@example.com",
                "IncorrectPassword123",
                _settings.DashboardPath,
                responseTimeout: TimeSpan.FromSeconds(15),
                redirectTimeout: TimeSpan.FromSeconds(5));

            ExtentTest.Info($"Network: {result.StatusCode?.ToString() ?? "no response captured"}");
            LoginAssertions.AssertLoginFailed(result, _settings.LoginPath);
        }
    }
}
