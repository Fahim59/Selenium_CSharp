using Selenium_CSharp_Framework.factory;
using Selenium_CSharp_Framework.pages;
using Selenium_CSharp_Framework.utilities;

namespace Selenium_CSharp_Framework.tests
{
    public class EndToEndFlow : DriverFactory
    {
        private LoginPage _loginPage;

        [OneTimeSetUp]
        public void InitializePageObjects()
        {
            _loginPage = new LoginPage(Driver);
        }

        [Test]
        public void VerifyUserSuccessfulLogin()
        {
            string userName = EnvConfig.Username;
            string password = EnvConfig.Password;

            _loginPage.Login(userName, password, "admin", "Consultant");

            _loginPage.VisibilityOfElements();
        }

        //------------------------------------------Using TestCase----------------------------------------------------------//

        //[Test(Description = "Verify that a user can log in successfully")]
        //[Category("SmokeTest")]
        //[TestCase("admin", "Consultant")]
        //[TestCase("admin", "Student")]
        //[Order(1)]
        //public void VerifyUserSuccessfulLogin(string userType, string personType)
        //{
        //    string userName = EnvConfig.Username;
        //    string password = EnvConfig.Password;

        //    _loginPage.Login(userName, password, userType, personType);
        //}

        //------------------------------------------Using TestCaseSource----------------------------------------------------------//

        //[Test, TestCaseSource(nameof(AddTestDataConfig))]
        //public void VerifyUserSuccessfulLogin(string userType, string personType)
        //{
        //    string userName = EnvConfig.Username;
        //    string password = EnvConfig.Password;

        //    _loginPage.Login(userName, password, userType, personType);
        //}

        //public static IEnumerable<TestCaseData> AddTestDataConfig()
        //{
        //    yield return new TestCaseData("admin", "Consultant").SetName("VerifyUserSuccessfulLogin_Consultant");
        //    yield return new TestCaseData("admin", "Student").SetName("VerifyUserSuccessfulLogin_Student");
        //}

        //---------------------------------------------Using Json File-----------------------------------------------------//

        //private static readonly List<LoginTestData> LoginData =
        //     JsonReader.ReadSection<LoginTestData>("resource/TestData.json", "login");

        //[Test, TestCaseSource(nameof(LoginData))]
        //public void VerifyUserSuccessfulLogin(LoginTestData data)
        //{
        //    string userName = EnvConfig.Username;
        //    string password = EnvConfig.Password;
        //    string userType = data.UserType;
        //    string personType = data.PersonType;    

        //    _loginPage.Login(userName, password, userType, personType);
        //}
    }
}