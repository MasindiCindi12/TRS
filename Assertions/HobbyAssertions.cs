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

        public static void AssertBlankFieldValidationMessages(ExtentTest test, HobbyValidationResult result)
        {
            test.Info("Expected: Both Name and Hobby Type show a validation message when left blank.");
            test.Info($"Actual: Name: {(result.NameMessage is null ? "none" : $"\"{result.NameMessage}\"")}, " +
                      $"Hobby Type: {(result.HobbyTypeMessage is null ? "none" : $"\"{result.HobbyTypeMessage}\"")}.");

            Assert.Multiple(() =>
            {
                Assert.That(result.NameMessage, Is.Not.Null.And.Not.Empty, "Expected a validation message for Name.");
                Assert.That(result.HobbyTypeMessage, Is.Not.Null.And.Not.Empty, "Expected a validation message for Hobby Type.");
            });
        }

        // Documents a real defect (see README's Known Application Defects section): the app allows
        // creating a second hobby with an identical name, and since Edit/Delete are matched by name
        // text (HobbiesPageLocators.EditButtonInRow/DeleteButtonInRow), duplicates make those actions
        // ambiguous. This is written to fail until the app rejects or clearly flags duplicate names.
        public static void AssertDuplicateNameRejected(ExtentTest test, string hobbyName, int rowCountAfterDuplicateAttempt)
        {
            test.Info($"Expected: Submitting Add Hobby again with the same name ('{hobbyName}') should be rejected " +
                      "or clearly flagged as a duplicate, not silently create a second identical entry.");
            test.Info($"Actual: {rowCountAfterDuplicateAttempt} row(s) named '{hobbyName}' exist after the duplicate submission.");

            Assert.That(rowCountAfterDuplicateAttempt, Is.EqualTo(1),
                $"Expected only one hobby named '{hobbyName}' to exist, but found {rowCountAfterDuplicateAttempt} — " +
                "duplicate hobby names are silently allowed, which makes Edit/Delete (matched by name text) ambiguous.");
        }
    }
}
