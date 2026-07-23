using AventStack.ExtentReports;
using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Assertions
{
    internal static class LogoutAssertions
    {
        public static void AssertLogoutSucceeded(ExtentTest test, LogoutResult result, string loginPath)
        {
            test.Info("Expected: Logging out redirects back to the Sign In page.");
            test.Info($"Actual: Final URL: {result.FinalUrl}.");

            Assert.That(result.FinalUrl, Does.Contain(loginPath),
                "Expected to be redirected to the Sign In page after logging out.");
        }
    }
}
