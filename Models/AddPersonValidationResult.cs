namespace TRS.Web.Automation.Models
{
    public record AddPersonValidationResult(
        string? FirstNameMessage,
        string? LastNameMessage,
        string? EmailMessage,
        string? PasswordMessage,
        string? PasswordConfirmMessage);
}
