using TRS.Web.Automation.Assertions;
using TRS.Web.Automation.Configuration;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Pages;

namespace TRS.Web.Automation.Tests
{
    [TestFixture]
    public class HobbyTests : BaseTest
    {
        private HobbiesPage _hobbiesPage = null!;
        private AppSettings _settings = null!;

        [SetUp]
        public async Task SetUpLoggedInSession()
        {
            _settings = AppSettingsProvider.Current;

            Assert.That(_settings.LoginEmail, Is.Not.Empty,
                "Set LoginEmail in Configuration/appsettings.local.json before running this test.");
            Assert.That(_settings.LoginPassword, Is.Not.Empty,
                "Set LoginPassword in Configuration/appsettings.local.json before running this test.");

            var loginPage = new LoginPage(Driver, ExtentTest, Recorder);
            _hobbiesPage = new HobbiesPage(Driver, ExtentTest, Recorder);

            ExtentTest.Info(PreConditionBanner);
            loginPage.NavigateTo(_settings.BaseUrl, _settings.LoginPath);

            var loginResult = await loginPage.SubmitLoginAsync(
                _settings.LoginEmail,
                _settings.LoginPassword,
                _settings.DashboardPath,
                responseTimeout: TimeSpan.FromSeconds(15),
                redirectTimeout: TimeSpan.FromSeconds(20));

            LoginAssertions.AssertLoginSucceeded(loginResult, _settings.DashboardPath);

            _hobbiesPage.NavigateTo(_settings.BaseUrl, _settings.HobbiesPath);
        }

        private static string UniqueHobbyName(string label)
        {
            var suffix = Guid.NewGuid().ToString("N")[..3];
            return $"{label} {suffix}";
        }

        [Test]
        [Category("Hobby Tab")]
        public void AddHobby_ShouldAppearInHobbiesList()
        {
            var hobbyName = UniqueHobbyName("Automation Hobby");

            var result = _hobbiesPage.SubmitAddHobby(hobbyName, "Sports");

            ExtentTest.Info($"Expected: '{hobbyName}' should appear in the hobbies list after adding it.");
            ExtentTest.Info($"Actual: Added hobby: {result.HobbyName} ({result.HobbyType}), listed: {result.IsListed}.");
            HobbyAssertions.AssertHobbyAdded(result);
        }

        [Test]
        [Category("Hobby Tab")]
        public void EditHobby_ShouldUpdateNameAndType()
        {
            var originalName = UniqueHobbyName("Automation Hobby");
            _hobbiesPage.SubmitAddHobby(originalName, "Sports");

            var updatedName = UniqueHobbyName("Automation Hobby Edited");
            var result = _hobbiesPage.SubmitEditHobby(originalName, updatedName, "Music");

            ExtentTest.Info($"Expected: '{updatedName}' should replace '{originalName}' in the hobbies list.");
            ExtentTest.Info($"Actual: Edited hobby: {result.UpdatedName} ({result.UpdatedType}), " +
                             $"updated listed: {result.UpdatedNameIsListed}, original still listed: {result.OriginalNameIsListed}.");
            HobbyAssertions.AssertHobbyEdited(result);
        }

        [Test]
        [Category("Hobby Tab")]
        public void DeleteHobby_ShouldRemoveHobbyFromList()
        {
            var hobbyName = UniqueHobbyName("Automation Hobby");
            _hobbiesPage.SubmitAddHobby(hobbyName, "Sports");

            var result = _hobbiesPage.SubmitDeleteHobby(hobbyName, _settings.BaseUrl, _settings.HobbiesPath);

            ExtentTest.Info($"Expected: '{hobbyName}' should no longer appear in the hobbies list after deleting it and reloading the page.");
            ExtentTest.Info($"Actual: Deleted hobby: {result.HobbyName}, still listed after reload: {result.StillListedAfterReload}.");
            HobbyAssertions.AssertHobbyDeleted(result);
        }
    }
}
