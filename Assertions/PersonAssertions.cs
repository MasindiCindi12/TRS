using TRS.Web.Automation.Models;

namespace TRS.Web.Automation.Assertions
{
    internal static class PersonAssertions
    {
        public static void AssertPersonAdded(AddPersonResult result)
        {
            Assert.That(result.IsListed, Is.True,
                $"Expected {result.Email} to appear in the People list after adding them.");
        }

        public static void AssertEditDialogDisplayed(EditPersonResult result)
        {
            Assert.That(result.EditDialogDisplayed, Is.True,
                $"Expected clicking Edit for {result.Email} to display an edit dialog.");
        }
    }
}
