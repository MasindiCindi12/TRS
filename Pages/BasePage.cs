using AventStack.ExtentReports;
using OpenQA.Selenium;
using TRS.Web.Automation.Utilities;

namespace TRS.Web.Automation.Pages
{
    public class BasePage
    {
        protected IWebDriver Driver;
        protected ExtentTest Test;

        private readonly ScreenRecorder _recorder;

        public BasePage(IWebDriver driver, ExtentTest test, ScreenRecorder recorder)
        {
            Driver = driver;
            Test = test;
            _recorder = recorder;
        }

        protected void LogStep(string message)
        {
            Test.Info(message);
            _recorder.CaptureFrame();
        }
    }
}
