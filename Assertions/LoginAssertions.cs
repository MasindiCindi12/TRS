using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Assertions
{
    internal static class LoginAssertions
    {
        public static void AssertLoginSucceeded(LoginResult result, string dashboardPath)
        {
            Assert.That(result.StatusCode, Is.EqualTo(200),
                "Expected the credentials callback to return 200 OK.");
            Assert.That(result.FinalUrl, Does.Contain(dashboardPath),
                "Expected to land on the dashboard after a successful login.");
        }

        public static void AssertLoginFailed(LoginResult result, string loginPath)
        {
            Assert.That(result.StatusCode, Is.EqualTo(401),
                "Expected the credentials callback to return 401 Unauthorized.");
            Assert.That(result.FinalUrl, Does.Contain(loginPath),
                "Expected to remain on the login page after a failed login.");
        }
    }
}
