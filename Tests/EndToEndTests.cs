using TRS.Web.Automation.Assertions;
using TRS.Web.Automation.Configuration;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Pages;
using TRS.Web.Automation.TestData;

namespace TRS.Web.Automation.Tests
{
    [TestFixture]
    public class EndToEndTests : BaseTest
    {
        private AppSettings _settings = null!;

        [SetUp]
        public void SetUpSettings()
        {
            _settings = AppSettingsProvider.Current;
        }

        [Test]
        [Category("End To End Tests")]
        public async Task CompleteUserJourney_ShouldSucceedFromSignUpToLogout()
        {
            var signUpPage = new SignUpPage(Driver, ExtentTest, Recorder);
            var loginPage = new LoginPage(Driver, ExtentTest, Recorder);
            var peoplePage = new PeoplePage(Driver, ExtentTest, Recorder);
            var hobbiesPage = new HobbiesPage(Driver, ExtentTest, Recorder);
            var dashboardPage = new DashboardPage(Driver, ExtentTest, Recorder);

            var password = TestDataFactory.DefaultPassword;
            var accountEmail = TestDataFactory.UniqueEmail("e2e");

            ExtentTest.Info(PreConditionBanner);

            // Sign Up
            signUpPage.NavigateTo(_settings.BaseUrl, _settings.SignUpPath);
            var signUpResult = signUpPage.SubmitSignUp(
                TestDataFactory.Names.EndToEndFirstName, TestDataFactory.Names.EndToEndLastName, accountEmail, password, _settings.LoginPath,
                redirectTimeout: TimeSpan.FromSeconds(15));

            // Login
            loginPage.NavigateTo(_settings.BaseUrl, _settings.LoginPath);
            var loginResult = await loginPage.SubmitLoginAsync(accountEmail, password, _settings.DashboardPath,
                responseTimeout: TimeSpan.FromSeconds(15), redirectTimeout: TimeSpan.FromSeconds(20));

            var statsBefore = dashboardPage.GetStats();

            // Add Person
            peoplePage.NavigateTo(_settings.BaseUrl, _settings.PeoplePath);
            var linkedPersonEmail = TestDataFactory.UniqueEmail("linked");
            var addPersonResult = peoplePage.SubmitAddPerson(
                TestDataFactory.Names.LinkedPersonFirstName, TestDataFactory.Names.LinkedPersonLastName, linkedPersonEmail, password);

            // Add Hobby (for the signed-up account itself)
            hobbiesPage.NavigateTo(_settings.BaseUrl, _settings.HobbiesPath);
            var ownHobbyName = TestDataFactory.UniqueName(TestDataFactory.HobbyNames.EndToEndOwn);
            var addHobbyResult = hobbiesPage.SubmitAddHobby(ownHobbyName, TestDataFactory.HobbyTypes.Sports);

            // Link Hobby (a new hobby created for and linked to the person just added)
            peoplePage.NavigateTo(_settings.BaseUrl, _settings.PeoplePath);
            var linkedHobbyName = TestDataFactory.UniqueName(TestDataFactory.HobbyNames.EndToEndLinked);
            peoplePage.SubmitLinkHobby(linkedHobbyName, TestDataFactory.HobbyTypes.Music);

            // Verify Dashboard statistics
            dashboardPage.NavigateTo(_settings.BaseUrl, _settings.DashboardPath);
            var statsAfter = dashboardPage.GetStats();
            ExtentTest.Info($"Dashboard before: {statsBefore.TotalUsers} users, {statsBefore.TotalHobbies} hobbies. " +
                             $"After: {statsAfter.TotalUsers} users, {statsAfter.TotalHobbies} hobbies.");

            // Logout
            var logoutResult = dashboardPage.SubmitLogOut(_settings.LoginPath, redirectTimeout: TimeSpan.FromSeconds(15));

            // Verify the linked hobby actually belongs to the linked person's own account
            loginPage.NavigateTo(_settings.BaseUrl, _settings.LoginPath);
            var linkedPersonLoginResult = await loginPage.SubmitLoginAsync(linkedPersonEmail, password, _settings.DashboardPath,
                responseTimeout: TimeSpan.FromSeconds(15), redirectTimeout: TimeSpan.FromSeconds(20));

            hobbiesPage.NavigateTo(_settings.BaseUrl, _settings.HobbiesPath);
            var linkedHobbyIsListed = hobbiesPage.IsHobbyListed(linkedHobbyName);

            ExtentTest.Info("Expected: Sign up redirects to the Sign In page.");
            ExtentTest.Info($"Actual: Final URL: {signUpResult.FinalUrl}.");

            ExtentTest.Info("Expected: Login succeeds and redirects to the dashboard.");
            ExtentTest.Info($"Actual: Network status {loginResult.StatusCode?.ToString() ?? "no response captured"}, final URL: {loginResult.FinalUrl}.");

            ExtentTest.Info($"Expected: '{linkedPersonEmail}' should appear in the People list after adding them.");
            ExtentTest.Info($"Actual: Added person: {addPersonResult.Email}, listed: {addPersonResult.IsListed}.");

            ExtentTest.Info($"Expected: '{ownHobbyName}' should appear in the hobbies list after adding it.");
            ExtentTest.Info($"Actual: Added hobby: {addHobbyResult.HobbyName} ({addHobbyResult.HobbyType}), listed: {addHobbyResult.IsListed}.");

            ExtentTest.Info("Expected: Dashboard should render well-formed, non-negative statistics.");
            ExtentTest.Info($"Actual: Total Users: {statsAfter.TotalUsers}, Total Hobbies: {statsAfter.TotalHobbies}.");

            ExtentTest.Info("Expected: Logging out redirects back to the Sign In page.");
            ExtentTest.Info($"Actual: Final URL: {logoutResult.FinalUrl}.");

            ExtentTest.Info($"Expected: '{linkedPersonEmail}' should be able to log in successfully.");
            ExtentTest.Info($"Actual: Network status {linkedPersonLoginResult.StatusCode?.ToString() ?? "no response captured"}, final URL: {linkedPersonLoginResult.FinalUrl}.");

            ExtentTest.Info($"Expected: '{linkedHobbyName}' should appear on the linked person's own Hobbies list after Link Hobby.");
            ExtentTest.Info($"Actual: Linked hobby listed: {linkedHobbyIsListed}.");

            Assert.Multiple(() =>
            {
                SignUpAssertions.AssertSignUpSucceeded(signUpResult, _settings.LoginPath);
                LoginAssertions.AssertLoginSucceeded(loginResult, _settings.DashboardPath);
                PersonAssertions.AssertPersonAdded(addPersonResult);
                HobbyAssertions.AssertHobbyAdded(addHobbyResult);
                DashboardAssertions.AssertStatsAreWellFormed(statsAfter);
                LogoutAssertions.AssertLogoutSucceeded(logoutResult, _settings.LoginPath);
                LoginAssertions.AssertLoginSucceeded(linkedPersonLoginResult, _settings.DashboardPath);
                Assert.That(linkedHobbyIsListed, Is.True,
                    $"Expected '{linkedHobbyName}' to appear on the linked person's own Hobbies list after Link Hobby, " +
                    "but it does not — the hobby was never actually created/linked.");
            });
        }
    }
}
