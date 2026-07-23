using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Objects;
using TRS.Web.Automation.Utilities;

namespace TRS.Web.Automation.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(IWebDriver driver, ExtentTest test, ScreenRecorder recorder) : base(driver, test, recorder)
        {
        }

        public void OpenUserMenu()
        {
            Driver.FindElement(DashboardPageLocators.UserMenuTrigger).Click();
            LogStep("User Menu Opened.");
        }

        public void ClickLogOut()
        {
            Driver.FindElement(DashboardPageLocators.LogOutMenuItem).Click();
            LogStep("Log Out Clicked.");
        }

        public void LogOut()
        {
            OpenUserMenu();
            ClickLogOut();
        }

        public LogoutResult SubmitLogOut(string loginPath, TimeSpan redirectTimeout)
        {
            LogOut();

            try
            {
                WaitHelper.WaitUntilUrlContains(Driver, loginPath, redirectTimeout);
                LogStep("Log out complete — redirected to the Sign In page.");
            }
            catch (WebDriverTimeoutException)
            {
                LogStep("Log out did not redirect to the Sign In page in time.");
            }

            return new LogoutResult(Driver.Url);
        }
    }
}
