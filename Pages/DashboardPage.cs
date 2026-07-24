using AventStack.ExtentReports;
using System.Linq;
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

        public void NavigateTo(string baseUrl, string dashboardPath)
        {
            Driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}{dashboardPath}");
            LogStep($"Navigated to the Dashboard Page ({Driver.Title}).");
        }

        public DashboardStats GetStats()
        {
            var totalUsers = int.Parse(Driver.FindElement(DashboardPageLocators.StatValue("Total Users")).Text);
            var totalHobbies = int.Parse(Driver.FindElement(DashboardPageLocators.StatValue("Total Hobbies")).Text);
            var stats = new DashboardStats(totalUsers, totalHobbies);
            LogStep($"Dashboard stats: {stats.TotalUsers} users, {stats.TotalHobbies} hobbies.");
            return stats;
        }

        public int GetHobbyDistributionCount(string hobbyType)
        {
            var element = Driver.FindElements(DashboardPageLocators.HobbyDistributionCount(hobbyType)).FirstOrDefault();
            var count = element is null ? 0 : int.Parse(element.Text);
            LogStep($"Hobby Distribution count for {hobbyType}: {count}.");
            return count;
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
