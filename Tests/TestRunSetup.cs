using TRS.Web.Automation.Utilities;

namespace TRS.Web.Automation.Tests
{
    [SetUpFixture]
    public class TestRunSetup
    {
        [OneTimeSetUp]
        public void GlobalSetUp()
        {
            _ = ExtentReportManager.Instance;
        }

        [OneTimeTearDown]
        public void GlobalTearDown()
        {
            ExtentReportManager.Flush();
        }
    }
}
