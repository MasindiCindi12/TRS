using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Assertions
{
    internal static class HobbyAssertions
    {
        public static void AssertHobbyAdded(AddHobbyResult result)
        {
            Assert.That(result.IsListed, Is.True,
                $"Expected {result.HobbyName} ({result.HobbyType}) to appear in the hobbies list after adding it.");
        }

        public static void AssertHobbyEdited(EditHobbyResult result)
        {
            Assert.That(result.UpdatedNameIsListed, Is.True,
                $"Expected the hobby's updated name '{result.UpdatedName}' ({result.UpdatedType}) to appear in the list.");
            Assert.That(result.OriginalNameIsListed, Is.False,
                "Expected the hobby's original name to no longer be listed after editing.");
        }

        public static void AssertHobbyDeleted(DeleteHobbyResult result)
        {
            Assert.That(result.StillListedAfterReload, Is.False,
                $"Expected {result.HobbyName} to be removed from the hobbies list after deleting, " +
                "but it is still listed after a reload.");
        }
    }
}
