using AventStack.ExtentReports;
using OpenQA.Selenium;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Objects;
using TRS.Web.Automation.Utilities;

namespace TRS.Web.Automation.Pages
{
    public class PeoplePage : BasePage
    {
        public PeoplePage(IWebDriver driver, ExtentTest test, ScreenRecorder recorder) : base(driver, test, recorder)
        {
        }

        public void NavigateTo(string baseUrl, string peoplePath)
        {
            Driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}{peoplePath}");
            LogStep($"Navigated to the People Page ({Driver.Title}).");
        }

        public string GetFirstRowEmail()
        {
            var email = Driver.FindElement(PeoplePageLocators.FirstRowEmailCell).Text;
            LogStep($"First row user email captured: {email}.");
            return email;
        }

        public void SelectFirstRow()
        {
            Driver.FindElement(PeoplePageLocators.FirstRowCheckbox).Click();
            LogStep("First row selected.");
        }

        public void DeleteSelectedRows()
        {
            Driver.FindElement(PeoplePageLocators.DeleteSelectedButton).Click();
            LogStep("Delete Selected Rows Clicked.");
        }

        public bool IsEmailListed(string email)
        {
            return Driver.FindElement(PeoplePageLocators.TableBody).Text.Contains(email);
        }

        public DeleteUserResult SubmitDeleteFirstUser(string baseUrl, string peoplePath)
        {
            var email = GetFirstRowEmail();

            SelectFirstRow();
            DeleteSelectedRows();

            NavigateTo(baseUrl, peoplePath);
            var stillListed = IsEmailListed(email);
            LogStep(stillListed
                ? $"User {email} is still listed after reload."
                : $"User {email} is no longer listed after reload.");

            return new DeleteUserResult(email, stillListed);
        }
    }
}
