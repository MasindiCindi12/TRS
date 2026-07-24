using OpenQA.Selenium;

namespace TRS.Web.Automation.Objects
{
    public static class HobbiesPageLocators
    {
        public static readonly By AddHobbyButton = By.XPath("//button[contains(., 'Add Hoby')]");
        public static readonly By NameInput = By.CssSelector("[data-cy='input-name']");
        public static readonly By HobbyTypeCombobox = By.CssSelector("[role='dialog'] button[role='combobox']");
        public static readonly By SubmitButton = By.CssSelector("[data-cy='submit-button']");
        public static readonly By ConfirmDeleteButton = By.XPath("//div[@role='alertdialog']//button[text()='Yes']");
        public static readonly By NameValidationMessage = By.CssSelector("[data-cy='input-name'] + p");
        public static readonly By HobbyTypeValidationMessage = By.CssSelector("[role='dialog'] select[aria-hidden='true'] + p");

        public static By HobbyTypeOption(string hobbyType) =>
            By.CssSelector($"[data-cy='select-item-{hobbyType}']");

        public static By HobbyRow(string hobbyName) =>
            By.XPath($"//div[contains(@class,'my-4')][.//span[text()='{hobbyName}']]");

        public static By EditButtonInRow(string hobbyName) =>
            By.XPath($"//div[contains(@class,'my-4')][.//span[text()='{hobbyName}']]//button[text()='Edit']");

        public static By DeleteButtonInRow(string hobbyName) =>
            By.XPath($"//div[contains(@class,'my-4')][.//span[text()='{hobbyName}']]//button[text()='Delete']");
    }
}
