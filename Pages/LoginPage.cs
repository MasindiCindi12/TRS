using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Objects;
using TRS.Web.Automation.Utilities;

namespace TRS.Web.Automation.Pages
{
    public class LoginPage : BasePage
    {
        private const string CredentialsCallbackPath = "/api/auth/callback/credentials";

        public LoginPage(IWebDriver driver, ExtentTest test, ScreenRecorder recorder) : base(driver, test, recorder)
        {
        }

        public void NavigateTo(string baseUrl, string loginPath)
        {
            Driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}{loginPath}");
            LogStep($"Navigated to the Sign In Page ({Driver.Title}).");
        }

        public void EnterEmail(string email)
        {
            Driver.FindElement(LoginPageLocators.EmailInput).SendKeys(email);
            LogStep("User Email Entered.");
        }

        public void EnterPassword(string password)
        {
            Driver.FindElement(LoginPageLocators.PasswordInput).SendKeys(password);
            LogStep("User Password Entered.");
        }

        public void Submit()
        {
            var button = Driver.FindElement(LoginPageLocators.SubmitButton);
            var label = string.IsNullOrWhiteSpace(button.Text) ? "Submit" : button.Text;
            button.Click();
            LogStep($"{label} Clicked.");
        }

        public void Login(string email, string password)
        {
            EnterEmail(email);
            EnterPassword(password);
            Submit();
        }

        public async Task<LoginResult> SubmitLoginAsync(
            string email,
            string password,
            string dashboardPath,
            TimeSpan responseTimeout,
            TimeSpan redirectTimeout)
        {
            var network = new NetworkCapture(Driver);
            await network.StartAsync();

            Login(email, password);
            var statusCode = await network.WaitForStatusCodeAsync(CredentialsCallbackPath, responseTimeout);

            if (statusCode == 200)
            {
                try
                {
                    WaitHelper.WaitUntilUrlContains(Driver, dashboardPath, redirectTimeout);
                    LogStep("Login complete — dashboard loaded.");
                }
                catch (WebDriverTimeoutException)
                {
                    LogStep("Login accepted (200) but the dashboard did not load in time.");
                }
            }
            else
            {
                LogStep($"Login failed — network status: {statusCode?.ToString() ?? "no response captured"}.");
            }

            await network.StopAsync();

            return new LoginResult(statusCode, Driver.Url);
        }
    }
}
