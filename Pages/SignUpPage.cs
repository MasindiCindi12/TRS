using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Utilities;

namespace TRS.Web.Automation.Pages
{
    public class SignUpPage : BasePage
    {
        private static readonly By FirstNameInput = By.CssSelector("[data-cy='input-first-name']");
        private static readonly By LastNameInput = By.CssSelector("[data-cy='input-last-name']");
        private static readonly By EmailInput = By.CssSelector("[data-cy='input-email']");
        private static readonly By PasswordInput = By.CssSelector("[data-cy='input-password']");
        private static readonly By ConfirmPasswordInput = By.CssSelector("[data-cy='input-password-confirm']");
        private static readonly By SubmitButton = By.CssSelector("[data-cy='submit-button']");

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
            Driver.FindElement(FirstNameInput).SendKeys(firstName);
            LogStep("First Name Entered.");
        }

        public void EnterLastName(string lastName)
        {
            Driver.FindElement(LastNameInput).SendKeys(lastName);
            LogStep("Last Name Entered.");
        }

        public void EnterEmail(string email)
        {
            Driver.FindElement(EmailInput).SendKeys(email);
            LogStep("Email Entered.");
        }

        public void EnterPassword(string password)
        {
            Driver.FindElement(PasswordInput).SendKeys(password);
            LogStep("Password Entered.");
        }

        public void EnterConfirmPassword(string password)
        {
            Driver.FindElement(ConfirmPasswordInput).SendKeys(password);
            LogStep("Confirm Password Entered.");
        }

        public void Submit()
        {
            var button = Driver.FindElement(SubmitButton);
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

        public SignUpResult SubmitSignUp(
            string firstName,
            string lastName,
            string email,
            string password,
            string loginPath,
            TimeSpan redirectTimeout)
        {
            SignUp(firstName, lastName, email, password);

            try
            {
                WaitHelper.WaitUntilUrlContains(Driver, loginPath, redirectTimeout);
                LogStep("Sign up complete — redirected to the Sign In page.");
            }
            catch (WebDriverTimeoutException)
            {
                LogStep("Sign up did not redirect to the Sign In page in time.");
            }

            return new SignUpResult(Driver.Url);
        }
    }
}
