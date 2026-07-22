using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TRS.Web.Automation.Utilities
{
    internal static class WaitHelper
    {
        public static void WaitUntilUrlChangesFrom(IWebDriver driver, string originalUrl, TimeSpan timeout)
        {
            var wait = new WebDriverWait(driver, timeout);
            wait.Until(d => d.Url != originalUrl);
        }
    }
}
