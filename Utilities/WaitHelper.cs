using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TRS.Web.Automation.Utilities
{
    internal static class WaitHelper
    {
        public static void WaitUntilUrlContains(IWebDriver driver, string substring, TimeSpan timeout)
        {
            var wait = new WebDriverWait(driver, timeout);
            wait.Until(d => d.Url.Contains(substring, StringComparison.OrdinalIgnoreCase));
        }
    }
}
