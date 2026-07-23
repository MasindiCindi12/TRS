using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Assertions
{
    internal static class DeleteUserAssertions
    {
        public static void AssertUserWasDeleted(DeleteUserResult result)
        {
            Assert.That(result.StillListedAfterReload, Is.False,
                $"Expected {result.DeletedEmail} to be removed from the People list after deleting, " +
                "but the user is still listed after a reload.");
        }
    }
}
