using AventStack.ExtentReports;
using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Assertions
{
    internal static class SignUpAssertions
    {
        public static void AssertSignUpSucceeded(ExtentTest test, SignUpResult result, string loginPath)
        {
            test.Info("Expected: Signing up with a new unique email redirects to the Sign In page.");
            test.Info($"Actual: Final URL: {result.FinalUrl}.");

            Assert.That(result.FinalUrl, Does.Contain(loginPath),
                "Expected to be redirected to the Sign In page after a successful sign up.");
        }

        public static void AssertPasswordValidationMatches(
            ExtentTest test,
            SignUpResult result,
            bool expectedAllowed,
            string signUpPath,
            string loginPath)
        {
            test.Info($"Expected: Registration is {(expectedAllowed ? "allowed" : "rejected")} for this password.");
            test.Info($"Actual: Final URL: {result.FinalUrl}. " +
                      $"Validation message: {(result.PasswordValidationMessage is null ? "none shown" : $"\"{result.PasswordValidationMessage}\"")}.");

            if (expectedAllowed)
            {
                Assert.That(result.FinalUrl, Does.Contain(loginPath),
                    "Expected registration to succeed (redirect to the Sign In page) for this password.");
            }
            else
            {
                Assert.That(result.FinalUrl, Does.Contain(signUpPath),
                    "Expected registration to be rejected (remain on the Sign Up page) for this password.");
            }
        }
    }
}
