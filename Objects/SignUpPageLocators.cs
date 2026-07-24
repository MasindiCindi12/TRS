using OpenQA.Selenium;

namespace TRS.Web.Automation.Objects
{
    public static class SignUpPageLocators
    {
        public static readonly By FirstNameInput = By.CssSelector("[data-cy='input-first-name']");
        public static readonly By LastNameInput = By.CssSelector("[data-cy='input-last-name']");
        public static readonly By EmailInput = By.CssSelector("[data-cy='input-email']");
        public static readonly By PasswordInput = By.CssSelector("[data-cy='input-password']");
        public static readonly By PasswordValidationMessage = By.CssSelector("[data-cy='input-password'] + p");
        public static readonly By ConfirmPasswordInput = By.CssSelector("[data-cy='input-password-confirm']");
        public static readonly By SubmitButton = By.CssSelector("[data-cy='submit-button']");
    }
}
