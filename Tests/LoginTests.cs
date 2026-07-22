using TRS.Web.Automation.Configuration;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Pages;
using TRS.Web.Automation.Utilities;

namespace TRS.Web.Automation.Tests
{
    [TestFixture]
    public class LoginTests : BaseTest
    {
        private LoginPage _loginPage = null!;
        private AppSettings _settings = null!;

        [SetUp]
        public void SetUpLoginPage()
        {
            _settings = AppSettingsProvider.Current;
            _loginPage = new LoginPage(Driver);
            _loginPage.NavigateTo(_settings.BaseUrl, _settings.LoginPath);
        }

        [Test]
        public void Login_WithValidCredentials_NavigatesAwayFromLoginPage()
        {
            Assert.That(_settings.LoginEmail, Is.Not.Empty,
                "Set LoginEmail in Configuration/appsettings.local.json before running this test.");
            Assert.That(_settings.LoginPassword, Is.Not.Empty,
                "Set LoginPassword in Configuration/appsettings.local.json before running this test.");

            var loginUrl = Driver.Url;

            _loginPage.Login(_settings.LoginEmail, _settings.LoginPassword);
            WaitHelper.WaitUntilUrlChangesFrom(Driver, loginUrl, TimeSpan.FromSeconds(10));

            Assert.That(Driver.Url, Does.Not.Contain(_settings.LoginPath));
        }
    }
}
