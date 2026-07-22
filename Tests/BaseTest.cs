using AventStack.ExtentReports;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TRS.Web.Automation.Utilities;

namespace TRS.Web.Automation.Tests
{
    public abstract class BaseTest
    {
        protected IWebDriver Driver { get; private set; } = null!;
        protected ExtentTest ExtentTest { get; private set; } = null!;

        [SetUp]
        public void SetUpDriver()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
            ExtentTest = ExtentReportManager.Instance.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void TearDownDriver()
        {
            LogResultToExtent();
            Driver.Quit();
            Driver.Dispose();
        }

        private void LogResultToExtent()
        {
            var result = TestContext.CurrentContext.Result;
            switch (result.Outcome.Status)
            {
                case TestStatus.Passed:
                    ExtentTest.Pass("Test passed");
                    break;
                case TestStatus.Failed:
                    ExtentTest.Fail(result.Message ?? "Test failed");
                    var screenshot = ((ITakesScreenshot)Driver).GetScreenshot().AsBase64EncodedString;
                    ExtentTest.AddScreenCaptureFromBase64String(screenshot);
                    break;
                default:
                    ExtentTest.Skip(result.Message ?? "Test skipped");
                    break;
            }
        }
    }
}
