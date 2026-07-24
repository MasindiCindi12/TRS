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
            var uniqueEmail = TestDataFactory.UniqueEmail(TestDataFactory.EmailLabels.SignUp);
            ExtentTest.Info($"Generated unique sign-up email: {uniqueEmail}");

            var result = _signUpPage.SubmitSignUp(
                TestDataFactory.Names.SignUpFirstName,
                TestDataFactory.Names.SignUpLastName,
                uniqueEmail,
                TestDataFactory.DefaultPassword,
                _settings.LoginPath,
                redirectTimeout: TimeSpan.FromSeconds(15));

            SignUpAssertions.AssertSignUpSucceeded(ExtentTest, result, _settings.LoginPath);
        }

        [TestCase(TestDataFactory.Passwords.MeetsAllRequirements, true,
            TestName = "SignUp_PasswordValidation_MeetsAllRequirements_IsAccepted")]
        [TestCase(TestDataFactory.Passwords.TooShort, false,
            TestName = "SignUp_PasswordValidation_TooShort_IsRejected")]
        [TestCase(TestDataFactory.Passwords.NoUppercase, true,
            TestName = "SignUp_PasswordValidation_NoUppercase_IsStillAccepted")]
        [TestCase(TestDataFactory.Passwords.NoLowercase, true,
            TestName = "SignUp_PasswordValidation_NoLowercase_IsStillAccepted")]
        [TestCase(TestDataFactory.Passwords.NoSpecialCharacter, false,
            TestName = "SignUp_PasswordValidation_NoSpecialCharacter_IsRejected")]
        [TestCase(TestDataFactory.Passwords.NoNumber, false,
            TestName = "SignUp_PasswordValidation_NoNumber_IsRejected")]
        [TestCase(TestDataFactory.Passwords.NoLetterAtAll, false,
            TestName = "SignUp_PasswordValidation_NoLetterAtAll_IsRejected")]
        [Category("Sign Up Tests")]
        public void SignUp_PasswordValidation_MatchesObservedRules(string password, bool expectedAllowed)
        {
            var uniqueEmail = TestDataFactory.UniqueEmail(TestDataFactory.EmailLabels.SignUp);
            ExtentTest.Info($"Generated unique sign-up email: {uniqueEmail}");
            ExtentTest.Info($"Password under test: \"{password}\" — expected to be {(expectedAllowed ? "allowed" : "rejected")}.");

            var result = _signUpPage.SubmitSignUp(
                TestDataFactory.Names.SignUpFirstName,
                TestDataFactory.Names.SignUpLastName,
                uniqueEmail,
                password,
                _settings.LoginPath,
                redirectTimeout: TimeSpan.FromSeconds(15));

            SignUpAssertions.AssertPasswordValidationMatches(
                ExtentTest, result, expectedAllowed, _settings.SignUpPath, _settings.LoginPath);
        }
    }
}
