using AventStack.ExtentReports;
using OpenQA.Selenium;

namespace TRS.Web.Automation.Pages
{
    public class BasePage
    {
        protected IWebDriver Driver;
        protected ExtentTest Test;

        public BasePage(IWebDriver driver, ExtentTest test)
        {
            Driver = driver;
            Test = test;
        }

        protected void LogStep(string message) => Test.Info(message);
    }
}
