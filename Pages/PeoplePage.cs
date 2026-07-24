using AventStack.ExtentReports;
using System.Linq;
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

            var isListed = TryWaitUntilVisible(PeoplePageLocators.RowByEmail(email), DefaultTimeout);
            return new AddPersonResult(email, isListed);
        }

        public AddPersonValidationResult SubmitAddPersonBlank()
        {
            Driver.FindElement(PeoplePageLocators.AddPersonButton).Click();
            LogStep("Add Person Clicked.");

            WaitHelper.WaitUntilVisible(Driver, PeoplePageLocators.Dialog, DefaultTimeout);
            Driver.FindElement(PeoplePageLocators.SubmitButton).Click();
            LogStep("Submit Clicked with all fields left blank.");

            string? Message(By locator) => Driver.FindElements(locator).FirstOrDefault()?.Text;

            var result = new AddPersonValidationResult(
                Message(PeoplePageLocators.FirstNameValidationMessage),
                Message(PeoplePageLocators.LastNameValidationMessage),
                Message(PeoplePageLocators.EmailValidationMessage),
                Message(PeoplePageLocators.PasswordValidationMessage),
                Message(PeoplePageLocators.ConfirmPasswordValidationMessage));

            LogStep($"Validation messages — First Name: \"{result.FirstNameMessage ?? "none"}\", " +
                    $"Last Name: \"{result.LastNameMessage ?? "none"}\", Email: \"{result.EmailMessage ?? "none"}\", " +
                    $"Password: \"{result.PasswordMessage ?? "none"}\", Password Confirm: \"{result.PasswordConfirmMessage ?? "none"}\".");

            return result;
        }

        public DuplicateEmailResult SubmitAddPersonWithDuplicateEmail(string firstName, string lastName, string existingEmail, string password)
        {
            var countBefore = Driver.FindElements(PeoplePageLocators.RowByEmail(existingEmail)).Count;

            Driver.FindElement(PeoplePageLocators.AddPersonButton).Click();
            LogStep("Add Person Clicked.");

            WaitHelper.WaitUntilVisible(Driver, PeoplePageLocators.FirstNameInput, DefaultTimeout).SendKeys(firstName);
            Driver.FindElement(PeoplePageLocators.LastNameInput).SendKeys(lastName);
            Driver.FindElement(PeoplePageLocators.EmailInput).SendKeys(existingEmail);
            Driver.FindElement(PeoplePageLocators.PasswordInput).SendKeys(password);
            Driver.FindElement(PeoplePageLocators.ConfirmPasswordInput).SendKeys(password);
            Driver.FindElement(PeoplePageLocators.SubmitButton).Click();
            LogStep($"Submitted a second person using the already-registered email {existingEmail}.");

            var dialogClosed = TryWaitUntilInvisible(PeoplePageLocators.Dialog, DefaultTimeout);
            var countAfter = Driver.FindElements(PeoplePageLocators.RowByEmail(existingEmail)).Count;
            LogStep($"Rows matching {existingEmail}: {countBefore} before, {countAfter} after the duplicate submission.");

            return new DuplicateEmailResult(existingEmail, countBefore, countAfter, dialogClosed);
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

        private bool TryWaitUntilInvisible(By locator, TimeSpan timeout)
        {
            try
            {
                new WebDriverWait(Driver, timeout).Until(d => d.FindElements(locator).Count == 0);
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}
