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
    }
}
