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

            LoginAssertions.AssertLoginSucceeded(ExtentTest, loginResult, _settings.DashboardPath);

            ExtentTest.Info(EndPreConditionBanner);

            _hobbiesPage.NavigateTo(_settings.BaseUrl, _settings.HobbiesPath);
        }

        [Test]
        [Category("Hobby Tab")]
        public void AddHobby_ShouldAppearInHobbiesList()
        {
            var hobbyName = TestDataFactory.UniqueName(TestDataFactory.HobbyNames.Default);

            var result = _hobbiesPage.SubmitAddHobby(hobbyName, TestDataFactory.HobbyTypes.Sports);

            HobbyAssertions.AssertHobbyAdded(ExtentTest, result);
        }

        [Test]
        [Category("Hobby Tab")]
        public void EditHobby_ShouldUpdateNameAndType()
        {
            var originalName = TestDataFactory.UniqueName(TestDataFactory.HobbyNames.Default);
            _hobbiesPage.SubmitAddHobby(originalName, TestDataFactory.HobbyTypes.Sports);

            var updatedName = TestDataFactory.UniqueName(TestDataFactory.HobbyNames.DefaultEdited);
            var result = _hobbiesPage.SubmitEditHobby(originalName, updatedName, TestDataFactory.HobbyTypes.Music);

            HobbyAssertions.AssertHobbyEdited(ExtentTest, result);
        }

        [Test]
        [Category("Hobby Tab")]
        public void DeleteHobby_ShouldRemoveHobbyFromList()
        {
            var hobbyName = TestDataFactory.UniqueName(TestDataFactory.HobbyNames.Default);
            _hobbiesPage.SubmitAddHobby(hobbyName, TestDataFactory.HobbyTypes.Sports);

            var result = _hobbiesPage.SubmitDeleteHobby(hobbyName, _settings.BaseUrl, _settings.HobbiesPath);

            HobbyAssertions.AssertHobbyDeleted(ExtentTest, result);
        }
    }
}
