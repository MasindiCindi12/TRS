using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Assertions
{
    internal static class LogoutAssertions
    {
        public static void AssertLogoutSucceeded(LogoutResult result, string loginPath)
        {
            Assert.That(result.FinalUrl, Does.Contain(loginPath),
                "Expected to be redirected to the Sign In page after logging out.");
        }
    }
}
