using AventStack.ExtentReports;
using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Assertions
{
    internal static class DeleteUserAssertions
    {
        public static void AssertUserWasDeleted(ExtentTest test, DeleteUserResult result)
        {
            test.Info($"Expected: '{result.DeletedEmail}' should no longer appear in the People list after deleting them and reloading the page.");
            test.Info($"Actual: Deleted user: {result.DeletedEmail}, still listed after reload: {result.StillListedAfterReload}.");

            Assert.That(result.StillListedAfterReload, Is.False,
                $"Expected {result.DeletedEmail} to be removed from the People list after deleting, " +
                "but the user is still listed after a reload.");
        }
    }
}
