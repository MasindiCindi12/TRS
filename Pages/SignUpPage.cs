using AventStack.ExtentReports;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Objects;
using TRS.Web.Automation.Utilities;

namespace TRS.Web.Automation.Pages
{
    public class SignUpPage : BasePage
    {
        public SignUpPage(IWebDriver driver, ExtentTest test, ScreenRecorder recorder) : base(driver, test, recorder)
        {
        }

        public void NavigateTo(string baseUrl, string signUpPath)
        {
            Driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}{signUpPath}");
            LogStep($"Navigated to the Sign Up Page ({Driver.Title}).");
        }

        public void EnterFirstName(string firstName)
        {
            Driver.FindElement(SignUpPageLocators.FirstNameInput).SendKeys(firstName);
            LogStep("First Name Entered.");
        }

        public void EnterLastName(string lastName)
        {
            Driver.FindElement(SignUpPageLocators.LastNameInput).SendKeys(lastName);
            LogStep("Last Name Entered.");
        }

        public void EnterEmail(string email)
        {
            Driver.FindElement(SignUpPageLocators.EmailInput).SendKeys(email);
            LogStep("Email Entered.");
        }

        public void EnterPassword(string password)
        {
            Driver.FindElement(SignUpPageLocators.PasswordInput).SendKeys(password);
            LogStep("Password Entered.");
        }

        public void EnterConfirmPassword(string password)
        {
            Driver.FindElement(SignUpPageLocators.ConfirmPasswordInput).SendKeys(password);
            LogStep("Confirm Password Entered.");
        }

        public void Submit()
        {
            var button = Driver.FindElement(SignUpPageLocators.SubmitButton);
            var label = string.IsNullOrWhiteSpace(button.Text) ? "Submit" : button.Text;
            button.Click();
            LogStep($"{label} Clicked.");
        }

        public void SignUp(string firstName, string lastName, string email, string password)
        {
            EnterFirstName(firstName);
            EnterLastName(lastName);
            EnterEmail(email);
            EnterPassword(password);
            EnterConfirmPassword(password);
            Submit();
        }

        public string? GetPasswordValidationMessage()
        {
            var message = Driver.FindElements(SignUpPageLocators.PasswordValidationMessage).FirstOrDefault();
            return message?.Text;
        }

        public SignUpResult SubmitSignUp(
            string firstName,
            string lastName,
            string email,
            string password,
            string loginPath,
            TimeSpan redirectTimeout)
        {
            SignUp(firstName, lastName, email, password);

            var passwordValidationMessage = GetPasswordValidationMessage();
            if (passwordValidationMessage is not null)
            {
                LogStep($"Password validation message shown: \"{passwordValidationMessage}\"");
            }

            try
            {
                WaitHelper.WaitUntilUrlContains(Driver, loginPath, redirectTimeout);
                LogStep("Sign up complete — redirected to the Sign In page.");
            }
            catch (WebDriverTimeoutException)
            {
                LogStep("Sign up did not redirect to the Sign In page in time.");
            }

            return new SignUpResult(Driver.Url, passwordValidationMessage);
        }
    }
}
