using AventStack.ExtentReports;
using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Assertions
{
    internal static class HobbyAssertions
    {
        public static void AssertHobbyAdded(ExtentTest test, AddHobbyResult result)
        {
            test.Info($"Expected: '{result.HobbyName}' should appear in the hobbies list after adding it.");
            test.Info($"Actual: Added hobby: {result.HobbyName} ({result.HobbyType}), listed: {result.IsListed}.");

            Assert.That(result.IsListed, Is.True,
                $"Expected {result.HobbyName} ({result.HobbyType}) to appear in the hobbies list after adding it.");
        }

        public static void AssertHobbyEdited(ExtentTest test, EditHobbyResult result)
        {
            test.Info($"Expected: '{result.UpdatedName}' should replace '{result.OriginalName}' in the hobbies list.");
            test.Info($"Actual: Edited hobby: {result.UpdatedName} ({result.UpdatedType}), " +
                       $"updated listed: {result.UpdatedNameIsListed}, original still listed: {result.OriginalNameIsListed}.");

            Assert.That(result.UpdatedNameIsListed, Is.True,
                $"Expected the hobby's updated name '{result.UpdatedName}' ({result.UpdatedType}) to appear in the list.");
            Assert.That(result.OriginalNameIsListed, Is.False,
                "Expected the hobby's original name to no longer be listed after editing.");
        }

        public static void AssertHobbyDeleted(ExtentTest test, DeleteHobbyResult result)
        {
            test.Info($"Expected: '{result.HobbyName}' should no longer appear in the hobbies list after deleting it and reloading the page.");
            test.Info($"Actual: Deleted hobby: {result.HobbyName}, still listed after reload: {result.StillListedAfterReload}.");

            Assert.That(result.StillListedAfterReload, Is.False,
                $"Expected {result.HobbyName} to be removed from the hobbies list after deleting, " +
                "but it is still listed after a reload.");
        }
    }
}
