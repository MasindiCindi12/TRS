using OpenQA.Selenium;

namespace TRS.Web.Automation.Objects
{
    public static class PeoplePageLocators
    {
        public static readonly By AddPersonButton = By.XPath("//button[contains(., 'Add Person')]");
        public static readonly By LinkHobbyButton = By.XPath("//button[contains(., 'Link Hobby')]");
        public static readonly By LinkHobbyNameInput = By.CssSelector("[data-cy='input-name']");
        public static readonly By LinkHobbyTypeCombobox = By.XPath("//label[text()='Hobby Type']/following-sibling::button[@role='combobox']");
        public static readonly By LinkHobbyUserCombobox = By.XPath("//label[text()='User']/following-sibling::button[@role='combobox']");
        public static readonly By LastLinkHobbyUserOption = By.XPath("(//div[starts-with(@data-cy,'select-item-user-')])[last()]");
        public static readonly By FirstNameInput = By.CssSelector("[data-cy='input-first-name']");
        public static readonly By LastNameInput = By.CssSelector("[data-cy='input-last-name']");
        public static readonly By EmailInput = By.CssSelector("[data-cy='input-email']");
        public static readonly By PasswordInput = By.CssSelector("[data-cy='input-password']");
        public static readonly By ConfirmPasswordInput = By.CssSelector("[data-cy='input-password-confirm']");
        public static readonly By SubmitButton = By.CssSelector("[data-cy='submit-button']");
        public static readonly By Dialog = By.CssSelector("[role='dialog']");
        public static readonly By FirstNameValidationMessage = By.CssSelector("[data-cy='input-first-name'] + p");
        public static readonly By LastNameValidationMessage = By.CssSelector("[data-cy='input-last-name'] + p");
        public static readonly By EmailValidationMessage = By.CssSelector("[data-cy='input-email'] + p");
        public static readonly By PasswordValidationMessage = By.CssSelector("[data-cy='input-password'] + p");
        public static readonly By ConfirmPasswordValidationMessage = By.CssSelector("[data-cy='input-password-confirm'] + p");
        public static readonly By EditMenuItem = By.XPath("//a[@role='menuitem'][text()='Edit']");
        public static readonly By DeleteSelectedButton = By.CssSelector("button[aria-label='Delete selected rows']");
        public static readonly By TableBody = By.CssSelector("table tbody");

        public static By RowByEmail(string email) => By.XPath($"//tr[.//td[text()='{email}']]");

        public static By OpenMenuButtonForEmail(string email) =>
            By.XPath($"//tr[.//td[text()='{email}']]//button[@aria-label='Open menu']");

        public static By CheckboxForEmail(string email) =>
            By.XPath($"//tr[.//td[text()='{email}']]//button[@role='checkbox']");

        public static By LinkHobbyTypeOption(string hobbyType) =>
            By.CssSelector($"[data-cy='select-item-hobby-{hobbyType}']");
    }
}
