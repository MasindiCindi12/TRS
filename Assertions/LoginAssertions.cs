using AventStack.ExtentReports;
using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Assertions
{
    internal static class LoginAssertions
    {
        public static void AssertLoginSucceeded(ExtentTest test, LoginResult result, string dashboardPath)
        {
            test.Info("Expected: Login succeeds (200 OK) and redirects to the dashboard.");
            test.Info($"Actual: Network status {result.StatusCode?.ToString() ?? "no response captured"}, final URL: {result.FinalUrl}.");

            Assert.That(result.StatusCode, Is.EqualTo(200),
                "Expected the credentials callback to return 200 OK.");
            Assert.That(result.FinalUrl, Does.Contain(dashboardPath),
                "Expected to land on the dashboard after a successful login.");
        }

        public static void AssertLoginFailed(ExtentTest test, LoginResult result, string loginPath)
        {
            test.Info("Expected: Login is rejected (401 Unauthorized) and the user remains on the Sign In page.");
            test.Info($"Actual: Network status {result.StatusCode?.ToString() ?? "no response captured"}, final URL: {result.FinalUrl}.");

            Assert.That(result.StatusCode, Is.EqualTo(401),
                "Expected the credentials callback to return 401 Unauthorized.");
            Assert.That(result.FinalUrl, Does.Contain(loginPath),
                "Expected to remain on the login page after a failed login.");
        }
    }
}
