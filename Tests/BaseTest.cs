using System.Reflection;
using System.Runtime.InteropServices;
using AventStack.ExtentReports;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TRS.Web.Automation.Configuration;
using TRS.Web.Automation.Utilities;

namespace TRS.Web.Automation.Tests
{
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public abstract class BaseTest
    {
        protected const string PreConditionBanner =
            "====================================Pre-Condition====================================";

        protected IWebDriver Driver { get; private set; } = null!;
        protected ExtentTest ExtentTest { get; private set; } = null!;
        protected ScreenRecorder Recorder { get; private set; } = null!;

        [SetUp]
        public void SetUpDriver()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
            ExtentTest = ExtentReportManager.Instance.CreateTest(TestContext.CurrentContext.Test.Name);
            foreach (var category in TestContext.CurrentContext.Test.Properties["Category"])
            {
                ExtentTest.AssignCategory(category.ToString());
            }
            LogSystemInformation();
            Recorder = new ScreenRecorder(Driver);
        }

        private void LogSystemInformation()
        {
            var browserVersion = ((IHasCapabilities)Driver).Capabilities.GetCapability("browserVersion")?.ToString() ?? "Unknown";
            var seleniumVersion = typeof(IWebDriver).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                ?? typeof(IWebDriver).Assembly.GetName().Version?.ToString()
                ?? "Unknown";

            ExtentTest.Info("==================== System Information ====================");
            ExtentTest.Info($"Browser: Chrome {browserVersion}");
            ExtentTest.Info($"Selenium: {seleniumVersion}");
            ExtentTest.Info($".NET: {RuntimeInformation.FrameworkDescription}");
            ExtentTest.Info($"OS: {RuntimeInformation.OSDescription}");
            ExtentTest.Info($"Test Environment: {AppSettingsProvider.Current.BaseUrl}");
        }

        [TearDown]
        public async Task TearDownDriver()
        {
            try
            {
                await SaveRecordingToExtent();
                LogResultToExtent();
            }
            finally
            {
                Driver.Quit();
                Driver.Dispose();
                ExtentReportManager.Flush();
            }
        }

        private async Task SaveRecordingToExtent()
        {
            var fileNameSafeTestName = string.Join("_", TestContext.CurrentContext.Test.Name.Split(Path.GetInvalidFileNameChars()));
            var fileName = $"{fileNameSafeTestName}_{DateTime.UtcNow:yyyyMMddHHmmssfff}.gif";
            var outputPath = Path.Combine(ExtentReportManager.ReportsDirectory, "Recordings", fileName);

            var savedPath = await Recorder.SaveAsync(outputPath);
            if (savedPath is not null)
            {
                ExtentTest.AddScreenCaptureFromPath($"Recordings/{fileName}");
            }
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
