using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TRS.Web.Automation.Tests
{
    public abstract class BaseTest
    {
        protected IWebDriver Driver { get; private set; } = null!;

        [SetUp]
        public void SetUpDriver()
        {
            Driver = new ChromeDriver();
            Driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDownDriver()
        {
            Driver.Quit();
            Driver.Dispose();
        }
    }
}
