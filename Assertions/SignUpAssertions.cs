using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Assertions
{
    internal static class SignUpAssertions
    {
        public static void AssertSignUpSucceeded(SignUpResult result, string loginPath)
        {
            Assert.That(result.FinalUrl, Does.Contain(loginPath),
                "Expected to be redirected to the Sign In page after a successful sign up.");
        }
    }
}
