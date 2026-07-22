using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRS.Web.Automation.Pages
{
    public class BasePage
    {
        protected IWebDriver Driver;

        public BasePage(IWebDriver driver)
        {
            Driver = driver;
        }
    }
}
