using TRS.Web.Automation.Assertions;
using TRS.Web.Automation.Configuration;
using TRS.Web.Automation.Models;
using TRS.Web.Automation.Pages;
using TRS.Web.Automation.TestData;

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

            AssertCredentialsConfigured(_settings);

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

            LoginAssertions.AssertLoginSucceeded(ExtentTest, loginResult, _settings.DashboardPath);

            ExtentTest.Info(EndPreConditionBanner);

            _peoplePage.NavigateTo(_settings.BaseUrl, _settings.PeoplePath);
        }
        [Test]
        [Category("People Tab")]
        public void AddPerson_ShouldAppearInPeopleList()
        {
            var email = TestDataFactory.UniqueEmail(TestDataFactory.EmailLabels.Person);

            var result = _peoplePage.SubmitAddPerson(TestDataFactory.Names.PersonFirstName, TestDataFactory.Names.PersonLastName, email, TestDataFactory.DefaultPassword);

            PersonAssertions.AssertPersonAdded(ExtentTest, result);
        }

        [Test]
        [Category("People Tab")]
        public void EditPerson_WhenClicked_ShouldDisplayEditDialog()
        {
            var email = TestDataFactory.UniqueEmail(TestDataFactory.EmailLabels.Person);
            _peoplePage.SubmitAddPerson(TestDataFactory.Names.PersonFirstName, TestDataFactory.Names.PersonLastName, email, TestDataFactory.DefaultPassword);

            var result = _peoplePage.SubmitEditPerson(email);

            PersonAssertions.AssertEditDialogDisplayed(ExtentTest, result);
        }

        [Test]
        [Category("People Tab")]
        public void DeletePerson_WhenConfirmed_ShouldNotAppearAfterRefresh()
        {
            var email = TestDataFactory.UniqueEmail(TestDataFactory.EmailLabels.Person);
            var addResult = _peoplePage.SubmitAddPerson(TestDataFactory.Names.PersonFirstName, TestDataFactory.Names.PersonLastName, email, TestDataFactory.DefaultPassword);

            PersonAssertions.AssertPersonAdded(ExtentTest, addResult);

            var result = _peoplePage.SubmitDeleteUser(email, _settings.BaseUrl, _settings.PeoplePath);

            DeleteUserAssertions.AssertUserWasDeleted(ExtentTest, result);
        }

        [Test]
        [Category("People Tab")]
        public void AddPerson_WithBlankFields_ShouldShowValidationMessages()
        {
            var result = _peoplePage.SubmitAddPersonBlank();

            PersonAssertions.AssertBlankFieldValidationMessages(ExtentTest, result);
        }

        [Test]
        [Category("People Tab")]
        public void AddPerson_WithDuplicateEmail_ShouldNotCreateDuplicateRecord()
        {
            var email = TestDataFactory.UniqueEmail(TestDataFactory.EmailLabels.Person);
            var firstResult = _peoplePage.SubmitAddPerson(TestDataFactory.Names.PersonFirstName, TestDataFactory.Names.PersonLastName, email, TestDataFactory.DefaultPassword);
            PersonAssertions.AssertPersonAdded(ExtentTest, firstResult);

            var duplicateResult = _peoplePage.SubmitAddPersonWithDuplicateEmail("Duplicate", "Attempt", email, TestDataFactory.DefaultPassword);

            PersonAssertions.AssertDuplicateEmailNotCreated(ExtentTest, duplicateResult);
        }

        [TestCase(2, false, TestName = "AddPerson_NameBoundary_TwoCharacters_IsRejected")]
        [TestCase(3, true, TestName = "AddPerson_NameBoundary_ThreeCharacters_IsAccepted")]
        [Category("People Tab")]
        public void AddPerson_NameLengthBoundary_MatchesObservedRule(int nameLength, bool expectedAccepted)
        {
            var name = new string('B', nameLength);
            var email = TestDataFactory.UniqueEmail(TestDataFactory.EmailLabels.Person);
            ExtentTest.Info($"Testing a {nameLength}-character name (\"{name}\") — expected to be {(expectedAccepted ? "accepted" : "rejected")}.");

            var result = _peoplePage.SubmitAddPerson(name, name, email, TestDataFactory.DefaultPassword);

            PersonAssertions.AssertPersonAddedMatchesExpectation(ExtentTest, result, expectedAccepted);
        }
    }
}
