using OpenQA.Selenium;

namespace TRS.Web.Automation.Objects
{
    public static class PeoplePageLocators
    {
        public static readonly By FirstRowCheckbox = By.CssSelector("table tbody tr:first-child button[role='checkbox']");
        public static readonly By FirstRowEmailCell = By.CssSelector("table tbody tr:first-child td:nth-child(4)");
        public static readonly By DeleteSelectedButton = By.CssSelector("button[aria-label='Delete selected rows']");
        public static readonly By TableBody = By.CssSelector("table tbody");
    }
}
