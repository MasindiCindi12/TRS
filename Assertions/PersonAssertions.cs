using AventStack.ExtentReports;
using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Assertions
{
    internal static class PersonAssertions
    {
        public static void AssertPersonAdded(ExtentTest test, AddPersonResult result)
        {
            test.Info($"Expected: '{result.Email}' should appear in the People list after adding them.");
            test.Info($"Actual: Added person: {result.Email}, listed: {result.IsListed}.");

            Assert.That(result.IsListed, Is.True,
                $"Expected {result.Email} to appear in the People list after adding them.");
        }

        public static void AssertEditDialogDisplayed(ExtentTest test, EditPersonResult result)
        {
            test.Info($"Expected: Clicking Edit for '{result.Email}' in the People grid should display an edit dialog.");
            test.Info($"Actual: Edit clicked for {result.Email}, dialog displayed: {result.EditDialogDisplayed}.");

            Assert.That(result.EditDialogDisplayed, Is.True,
                $"Expected clicking Edit for {result.Email} to display an edit dialog.");
        }
    }
}
