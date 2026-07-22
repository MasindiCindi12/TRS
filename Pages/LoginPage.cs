using OpenQA.Selenium;

namespace TRS.Web.Automation.Pages
{
    public class LoginPage : BasePage
    {
        private static readonly By EmailInput = By.Name("email");
        private static readonly By PasswordInput = By.Name("password");
        private static readonly By SubmitButton = By.CssSelector("button[type='submit']");

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public void NavigateTo(string baseUrl, string loginPath)
        {
            Driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}{loginPath}");
        }

        public void EnterEmail(string email) => Driver.FindElement(EmailInput).SendKeys(email);

        public void EnterPassword(string password) => Driver.FindElement(PasswordInput).SendKeys(password);

        public void Submit() => Driver.FindElement(SubmitButton).Click();

        public void Login(string email, string password)
        {
            EnterEmail(email);
            EnterPassword(password);
            Submit();
        }
    }
}
