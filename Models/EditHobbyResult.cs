namespace TRS.Web.Automation.Models
{
    public record EditHobbyResult(string OriginalName, string UpdatedName, string UpdatedType, bool UpdatedNameIsListed, bool OriginalNameIsListed);
}
