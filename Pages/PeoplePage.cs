using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Objects;
using TRS.Web.Automation.Utilities;

namespace TRS.Web.Automation.Pages
{
    public class PeoplePage : BasePage
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);
        private static readonly TimeSpan EditDialogTimeout = TimeSpan.FromSeconds(5);

        public PeoplePage(IWebDriver driver, ExtentTest test, ScreenRecorder recorder) : base(driver, test, recorder)
        {
        }

        public void NavigateTo(string baseUrl, string peoplePath)
        {
            Driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}{peoplePath}");
            LogStep($"Navigated to the People Page ({Driver.Title}).");
        }

        public bool IsEmailListed(string email)
        {
            return Driver.FindElement(PeoplePageLocators.TableBody).Text.Contains(email);
        }

        public AddPersonResult SubmitAddPerson(string firstName, string lastName, string email, string password)
        {
            Driver.FindElement(PeoplePageLocators.AddPersonButton).Click();
            LogStep("Add Person Clicked.");

            WaitHelper.WaitUntilVisible(Driver, PeoplePageLocators.FirstNameInput, DefaultTimeout).SendKeys(firstName);
            Driver.FindElement(PeoplePageLocators.LastNameInput).SendKeys(lastName);
            Driver.FindElement(PeoplePageLocators.EmailInput).SendKeys(email);
            Driver.FindElement(PeoplePageLocators.PasswordInput).SendKeys(password);
            Driver.FindElement(PeoplePageLocators.ConfirmPasswordInput).SendKeys(password);
            Driver.FindElement(PeoplePageLocators.SubmitButton).Click();
            LogStep($"Person submitted: {firstName} {lastName} ({email}).");

            var isListed = WaitHelper.WaitUntilVisible(Driver, PeoplePageLocators.RowByEmail(email), DefaultTimeout) is not null;
            return new AddPersonResult(email, isListed);
        }

        public EditPersonResult SubmitEditPerson(string email)
        {
            Driver.FindElement(PeoplePageLocators.OpenMenuButtonForEmail(email)).Click();
            LogStep($"Open Menu Clicked for {email}.");

            WaitHelper.WaitUntilVisible(Driver, PeoplePageLocators.EditMenuItem, DefaultTimeout).Click();
            LogStep($"Edit Clicked for {email}.");

            var dialogDisplayed = TryWaitUntilVisible(PeoplePageLocators.Dialog, EditDialogTimeout);
            LogStep(dialogDisplayed
                ? $"Edit dialog displayed for {email}."
                : $"Edit dialog did not display for {email}.");

            return new EditPersonResult(email, dialogDisplayed);
        }

        public LinkHobbyResult SubmitLinkHobby(string hobbyName, string hobbyType)
        {
            Driver.FindElement(PeoplePageLocators.LinkHobbyButton).Click();
            LogStep("Link Hobby Clicked.");

            WaitHelper.WaitUntilVisible(Driver, PeoplePageLocators.LinkHobbyNameInput, DefaultTimeout).SendKeys(hobbyName);

            Driver.FindElement(PeoplePageLocators.LinkHobbyTypeCombobox).Click();
            WaitHelper.WaitUntilVisible(Driver, PeoplePageLocators.LinkHobbyTypeOption(hobbyType), DefaultTimeout).Click();

            Driver.FindElement(PeoplePageLocators.LinkHobbyUserCombobox).Click();
            var lastUserOption = WaitHelper.WaitUntilVisible(Driver, PeoplePageLocators.LastLinkHobbyUserOption, DefaultTimeout);
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", lastUserOption);
            lastUserOption.Click();

            new WebDriverWait(Driver, DefaultTimeout).Until(d => d.FindElements(By.CssSelector("[role='listbox']")).Count == 0);

            Driver.FindElement(PeoplePageLocators.SubmitButton).Click();
            LogStep($"Hobby linked: {hobbyName} ({hobbyType}) to the most recently added user.");

            return new LinkHobbyResult(hobbyName, hobbyType);
        }

        public DeleteUserResult SubmitDeleteUser(string email, string baseUrl, string peoplePath)
        {
            Driver.FindElement(PeoplePageLocators.CheckboxForEmail(email)).Click();
            LogStep($"Selected row for {email}.");

            Driver.FindElement(PeoplePageLocators.DeleteSelectedButton).Click();
            LogStep("Delete Selected Rows Clicked.");

            NavigateTo(baseUrl, peoplePath);
            var stillListed = IsEmailListed(email);
            LogStep(stillListed
                ? $"User {email} is still listed after reload."
                : $"User {email} is no longer listed after reload.");

            return new DeleteUserResult(email, stillListed);
        }

        private bool TryWaitUntilVisible(By locator, TimeSpan timeout)
        {
            try
            {
                WaitHelper.WaitUntilVisible(Driver, locator, timeout);
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}
