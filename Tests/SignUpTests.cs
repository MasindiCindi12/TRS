using TRS.Web.Automation.Assertions;
using TRS.Web.Automation.Configuration;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Pages;

namespace TRS.Web.Automation.Tests
{
    [TestFixture]
    public class SignUpTests : BaseTest
    {
        private SignUpPage _signUpPage = null!;
        private AppSettings _settings = null!;

        [SetUp]
        public void SetUpSignUpPage()
        {
            _settings = AppSettingsProvider.Current;
            _signUpPage = new SignUpPage(Driver, ExtentTest, Recorder);

            ExtentTest.Info(PreConditionBanner);
            _signUpPage.NavigateTo(_settings.BaseUrl, _settings.SignUpPath);
        }

        [Test]
        public void SignUp_WithUniqueRandomEmail_RedirectsToSignIn()
        {
            var sixDigitSuffix = Math.Abs(Guid.NewGuid().GetHashCode() % 1_000_000).ToString("D6");
            var uniqueEmail = $"trs.test.{sixDigitSuffix}@example.com";
            ExtentTest.Info($"Generated unique sign-up email: {uniqueEmail}");

            var result = _signUpPage.SubmitSignUp(
                "Trs",
                "Tester",
                uniqueEmail,
                "TestPass123!",
                _settings.LoginPath,
                redirectTimeout: TimeSpan.FromSeconds(15));

            SignUpAssertions.AssertSignUpSucceeded(result, _settings.LoginPath);
        }
    }
}
