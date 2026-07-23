using OpenQA.Selenium;

namespace TRS.Web.Automation.Objects
{
    public static class PeoplePageLocators
    {
        public static readonly By AddPersonButton = By.XPath("//button[contains(., 'Add Person')]");
        public static readonly By FirstNameInput = By.CssSelector("[data-cy='input-first-name']");
        public static readonly By LastNameInput = By.CssSelector("[data-cy='input-last-name']");
        public static readonly By EmailInput = By.CssSelector("[data-cy='input-email']");
        public static readonly By PasswordInput = By.CssSelector("[data-cy='input-password']");
        public static readonly By ConfirmPasswordInput = By.CssSelector("[data-cy='input-password-confirm']");
        public static readonly By SubmitButton = By.CssSelector("[data-cy='submit-button']");
        public static readonly By Dialog = By.CssSelector("[role='dialog']");
        public static readonly By EditMenuItem = By.XPath("//a[@role='menuitem'][text()='Edit']");
        public static readonly By DeleteSelectedButton = By.CssSelector("button[aria-label='Delete selected rows']");
        public static readonly By TableBody = By.CssSelector("table tbody");

        public static By RowByEmail(string email) => By.XPath($"//tr[.//td[text()='{email}']]");

        public static By OpenMenuButtonForEmail(string email) =>
            By.XPath($"//tr[.//td[text()='{email}']]//button[@aria-label='Open menu']");

        public static By CheckboxForEmail(string email) =>
            By.XPath($"//tr[.//td[text()='{email}']]//button[@role='checkbox']");
    }
}
