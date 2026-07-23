using OpenQA.Selenium;

namespace TRS.Web.Automation.Objects
{
    public static class DashboardPageLocators
    {
        public static readonly By UserMenuTrigger = By.CssSelector("header button.rounded-full");
        public static readonly By LogOutMenuItem = By.XPath("//button[@role='menuitem'][contains(., 'Log Out')]");
    }
}
