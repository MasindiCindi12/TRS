using AventStack.ExtentReports;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Objects;
using TRS.Web.Automation.Utilities;

namespace TRS.Web.Automation.Pages
{
    public class HobbiesPage : BasePage
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

        public HobbiesPage(IWebDriver driver, ExtentTest test, ScreenRecorder recorder) : base(driver, test, recorder)
        {
        }

        public void NavigateTo(string baseUrl, string hobbiesPath)
        {
            Driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}{hobbiesPath}");
            LogStep($"Navigated to the Hobbies Page ({Driver.Title}).");
        }

        public bool IsHobbyListed(string hobbyName)
        {
            return Driver.FindElements(HobbiesPageLocators.HobbyRow(hobbyName)).Count > 0;
        }

        private void FillHobbyForm(string hobbyName, string hobbyType, string? prefilledName = null)
        {
            var nameInput = WaitHelper.WaitUntilVisible(Driver, HobbiesPageLocators.NameInput, DefaultTimeout);

            if (!string.IsNullOrEmpty(prefilledName))
            {
                new WebDriverWait(Driver, DefaultTimeout).Until(_ => nameInput.GetAttribute("value") == prefilledName);
            }

            nameInput.SendKeys(Keys.Control + "a");
            nameInput.SendKeys(Keys.Delete);
            nameInput.SendKeys(hobbyName);

            Driver.FindElement(HobbiesPageLocators.HobbyTypeCombobox).Click();
            WaitHelper.WaitUntilVisible(Driver, HobbiesPageLocators.HobbyTypeOption(hobbyType), DefaultTimeout).Click();

            Driver.FindElement(HobbiesPageLocators.SubmitButton).Click();
        }

        public AddHobbyResult SubmitAddHobby(string hobbyName, string hobbyType)
        {
            Driver.FindElement(HobbiesPageLocators.AddHobbyButton).Click();
            LogStep("Add Hoby Clicked.");

            FillHobbyForm(hobbyName, hobbyType);
            LogStep($"Hobby submitted: {hobbyName} ({hobbyType}).");

            var isListed = WaitHelper.WaitUntilVisible(Driver, HobbiesPageLocators.HobbyRow(hobbyName), DefaultTimeout) is not null;
            return new AddHobbyResult(hobbyName, hobbyType, isListed);
        }

        public EditHobbyResult SubmitEditHobby(string currentName, string updatedName, string updatedType)
        {
            Driver.FindElement(HobbiesPageLocators.EditButtonInRow(currentName)).Click();
            LogStep($"Edit Clicked for {currentName}.");

            FillHobbyForm(updatedName, updatedType, prefilledName: currentName);
            LogStep($"Hobby updated: {currentName} -> {updatedName} ({updatedType}).");

            var updatedNameIsListed = WaitHelper.WaitUntilVisible(Driver, HobbiesPageLocators.HobbyRow(updatedName), DefaultTimeout) is not null;
            var originalNameIsListed = IsHobbyListed(currentName);

            return new EditHobbyResult(currentName, updatedName, updatedType, updatedNameIsListed, originalNameIsListed);
        }

        public DeleteHobbyResult SubmitDeleteHobby(string hobbyName, string baseUrl, string hobbiesPath)
        {
            Driver.FindElement(HobbiesPageLocators.DeleteButtonInRow(hobbyName)).Click();
            LogStep($"Delete Clicked for {hobbyName}.");

            WaitHelper.WaitUntilVisible(Driver, HobbiesPageLocators.ConfirmDeleteButton, DefaultTimeout).Click();
            LogStep("Delete Confirmed.");

            new WebDriverWait(Driver, DefaultTimeout).Until(_ => !IsHobbyListed(hobbyName));
            LogStep($"Hobby {hobbyName} removed from the client-side list.");

            NavigateTo(baseUrl, hobbiesPath);
            var stillListed = IsHobbyListed(hobbyName);
            LogStep(stillListed
                ? $"Hobby {hobbyName} is still listed after reload."
                : $"Hobby {hobbyName} is no longer listed after reload.");

            return new DeleteHobbyResult(hobbyName, stillListed);
        }
    }
}
