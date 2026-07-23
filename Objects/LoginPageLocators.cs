using OpenQA.Selenium;

namespace TRS.Web.Automation.Objects
{
    public static class LoginPageLocators
    {
        public static readonly By EmailInput = By.Name("email");
        public static readonly By PasswordInput = By.Name("password");
        public static readonly By SubmitButton = By.CssSelector("[data-cy='login-submit']");
    }
}
