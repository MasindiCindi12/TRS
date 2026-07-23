using TRS.Web.Automation.Assertions;
using TRS.Web.Automation.Configuration;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Pages;
using TRS.Web.Automation.TestData;

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

            AssertCredentialsConfigured(_settings);

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

        [Test]
        [Category("Hobby Tab")]
        public void AddHobby_ShouldAppearInHobbiesList()
        {
            var hobbyName = TestDataFactory.UniqueName(TestDataFactory.HobbyNames.Default);

            var result = _hobbiesPage.SubmitAddHobby(hobbyName, TestDataFactory.HobbyTypes.Sports);

            ExtentTest.Info($"Expected: '{hobbyName}' should appear in the hobbies list after adding it.");
            ExtentTest.Info($"Actual: Added hobby: {result.HobbyName} ({result.HobbyType}), listed: {result.IsListed}.");
            HobbyAssertions.AssertHobbyAdded(result);
        }

        [Test]
        [Category("Hobby Tab")]
        public void EditHobby_ShouldUpdateNameAndType()
        {
            var originalName = TestDataFactory.UniqueName(TestDataFactory.HobbyNames.Default);
            _hobbiesPage.SubmitAddHobby(originalName, TestDataFactory.HobbyTypes.Sports);

            var updatedName = TestDataFactory.UniqueName(TestDataFactory.HobbyNames.DefaultEdited);
            var result = _hobbiesPage.SubmitEditHobby(originalName, updatedName, TestDataFactory.HobbyTypes.Music);

            ExtentTest.Info($"Expected: '{updatedName}' should replace '{originalName}' in the hobbies list.");
            ExtentTest.Info($"Actual: Edited hobby: {result.UpdatedName} ({result.UpdatedType}), " +
                             $"updated listed: {result.UpdatedNameIsListed}, original still listed: {result.OriginalNameIsListed}.");
            HobbyAssertions.AssertHobbyEdited(result);
        }

        [Test]
        [Category("Hobby Tab")]
        public void DeleteHobby_ShouldRemoveHobbyFromList()
        {
            var hobbyName = TestDataFactory.UniqueName(TestDataFactory.HobbyNames.Default);
            _hobbiesPage.SubmitAddHobby(hobbyName, TestDataFactory.HobbyTypes.Sports);

            var result = _hobbiesPage.SubmitDeleteHobby(hobbyName, _settings.BaseUrl, _settings.HobbiesPath);

            ExtentTest.Info($"Expected: '{hobbyName}' should no longer appear in the hobbies list after deleting it and reloading the page.");
            ExtentTest.Info($"Actual: Deleted hobby: {result.HobbyName}, still listed after reload: {result.StillListedAfterReload}.");
            HobbyAssertions.AssertHobbyDeleted(result);
        }
    }
}
