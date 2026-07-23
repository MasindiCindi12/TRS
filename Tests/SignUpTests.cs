using TRS.Web.Automation.Assertions;
using TRS.Web.Automation.Configuration;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Pages;
using TRS.Web.Automation.TestData;

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
        [Category("Sign Up Tests")]
        public void SignUp_WithNewEmail_ShouldRedirectToSignIn()
        {
            var uniqueEmail = TestDataFactory.UniqueEmail("signup");
            ExtentTest.Info($"Generated unique sign-up email: {uniqueEmail}");

            var result = _signUpPage.SubmitSignUp(
                TestDataFactory.Names.SignUpFirstName,
                TestDataFactory.Names.SignUpLastName,
                uniqueEmail,
                TestDataFactory.DefaultPassword,
                _settings.LoginPath,
                redirectTimeout: TimeSpan.FromSeconds(15));

            ExtentTest.Info("Expected: Signing up with a new unique email redirects to the Sign In page.");
            ExtentTest.Info($"Actual: Final URL: {result.FinalUrl}.");
            SignUpAssertions.AssertSignUpSucceeded(result, _settings.LoginPath);
        }
    }
}
