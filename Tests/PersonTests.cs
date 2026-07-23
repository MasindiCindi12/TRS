using TRS.Web.Automation.Assertions;
using TRS.Web.Automation.Configuration;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Pages;

namespace TRS.Web.Automation.Tests
{
    [TestFixture]
    public class PersonTests : BaseTest
    {
        private PeoplePage _peoplePage = null!;
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
            _peoplePage = new PeoplePage(Driver, ExtentTest, Recorder);

            ExtentTest.Info(PreConditionBanner);
            loginPage.NavigateTo(_settings.BaseUrl, _settings.LoginPath);

            var loginResult = await loginPage.SubmitLoginAsync(
                _settings.LoginEmail,
                _settings.LoginPassword,
                _settings.DashboardPath,
                responseTimeout: TimeSpan.FromSeconds(15),
                redirectTimeout: TimeSpan.FromSeconds(20));

            LoginAssertions.AssertLoginSucceeded(loginResult, _settings.DashboardPath);

            _peoplePage.NavigateTo(_settings.BaseUrl, _settings.PeoplePath);
        }

        private static string UniquePersonEmail()
        {
            var sixDigitSuffix = Math.Abs(Guid.NewGuid().GetHashCode() % 1_000_000).ToString("D6");
            return $"trs.test.{sixDigitSuffix}@example.com";
        }

        [Test]
        [Category("People Tab")]
        public void AddPerson_ShouldAppearInPeopleList()
        {
            var email = UniquePersonEmail();

            var result = _peoplePage.SubmitAddPerson("Automation", "Person", email, "TestPass123!");

            ExtentTest.Info($"Expected: '{email}' should appear in the People list after adding them.");
            ExtentTest.Info($"Actual: Added person: {result.Email}, listed: {result.IsListed}.");
            PersonAssertions.AssertPersonAdded(result);
        }

        [Test]
        [Category("People Tab")]
        public void EditPerson_WhenClicked_ShouldDisplayEditDialog()
        {
            var email = UniquePersonEmail();
            _peoplePage.SubmitAddPerson("Automation", "Person", email, "TestPass123!");

            var result = _peoplePage.SubmitEditPerson(email);

            ExtentTest.Info($"Expected: Clicking Edit for '{email}' in the People grid should display an edit dialog.");
            ExtentTest.Info($"Actual: Edit clicked for {result.Email}, dialog displayed: {result.EditDialogDisplayed}.");
            PersonAssertions.AssertEditDialogDisplayed(result);
        }

        [Test]
        [Category("People Tab")]
        public void DeletePerson_WhenConfirmed_ShouldNotAppearAfterRefresh()
        {
            var email = UniquePersonEmail();
            var addResult = _peoplePage.SubmitAddPerson("Automation", "Person", email, "TestPass123!");

            ExtentTest.Info($"Expected: '{email}' should appear in the People list after adding them (precondition for delete).");
            ExtentTest.Info($"Actual: Added person: {addResult.Email}, listed: {addResult.IsListed}.");
            PersonAssertions.AssertPersonAdded(addResult);

            var result = _peoplePage.SubmitDeleteUser(email, _settings.BaseUrl, _settings.PeoplePath);

            ExtentTest.Info($"Expected: '{email}' should no longer appear in the People list after deleting them and reloading the page.");
            ExtentTest.Info($"Actual: Deleted user: {result.DeletedEmail}, still listed after reload: {result.StillListedAfterReload}.");
            DeleteUserAssertions.AssertUserWasDeleted(result);
        }
    }
}
