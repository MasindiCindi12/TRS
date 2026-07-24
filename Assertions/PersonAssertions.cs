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

        public static void AssertPersonAddedMatchesExpectation(ExtentTest test, AddPersonResult result, bool expectedAccepted)
        {
            test.Info($"Expected: This submission should be {(expectedAccepted ? "accepted" : "rejected")}.");
            test.Info($"Actual: '{result.Email}' listed: {result.IsListed}.");

            Assert.That(result.IsListed, Is.EqualTo(expectedAccepted),
                $"Expected '{result.Email}' listed status to be {expectedAccepted}, but it was {result.IsListed}.");
        }

        public static void AssertBlankFieldValidationMessages(ExtentTest test, AddPersonValidationResult result)
        {
            test.Info("Expected: First Name, Last Name, Email, and Password each show a validation message when left blank.");
            test.Info($"Actual: First Name: {Describe(result.FirstNameMessage)}, Last Name: {Describe(result.LastNameMessage)}, " +
                      $"Email: {Describe(result.EmailMessage)}, Password: {Describe(result.PasswordMessage)}, " +
                      $"Password Confirm: {Describe(result.PasswordConfirmMessage)}.");

            Assert.Multiple(() =>
            {
                Assert.That(result.FirstNameMessage, Is.Not.Null.And.Not.Empty, "Expected a validation message for First Name.");
                Assert.That(result.LastNameMessage, Is.Not.Null.And.Not.Empty, "Expected a validation message for Last Name.");
                Assert.That(result.EmailMessage, Is.Not.Null.And.Not.Empty, "Expected a validation message for Email.");
                Assert.That(result.PasswordMessage, Is.Not.Null.And.Not.Empty, "Expected a validation message for Password.");
            });
        }

        public static void AssertDuplicateEmailNotCreated(ExtentTest test, DuplicateEmailResult result)
        {
            test.Info($"Expected: Submitting Add Person with an email that already exists ('{result.Email}') should not create a second record.");
            test.Info($"Actual: Rows matching '{result.Email}': {result.RowCountBefore} before, {result.RowCountAfter} after. " +
                      $"Dialog closed: {result.DialogClosed}. Note: no error message is shown to the user either way — " +
                      "the duplicate is silently rejected rather than flagged (see OBS-004-style feedback gaps).");

            Assert.That(result.RowCountAfter, Is.EqualTo(result.RowCountBefore),
                $"Expected no duplicate record to be created for '{result.Email}', but the row count changed from " +
                $"{result.RowCountBefore} to {result.RowCountAfter}.");
        }

        private static string Describe(string? message) => message is null ? "none" : $"\"{message}\"";
    }
}
