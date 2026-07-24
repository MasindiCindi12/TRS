namespace TRS.Web.Automation.Models
{
    public record DuplicateEmailResult(string Email, int RowCountBefore, int RowCountAfter, bool DialogClosed);
}
