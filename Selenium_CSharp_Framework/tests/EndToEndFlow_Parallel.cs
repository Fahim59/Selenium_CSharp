using Selenium_CSharp_Framework.factory;
using Selenium_CSharp_Framework.pages;
using Selenium_CSharp_Framework.utilities;

namespace Selenium_CSharp_Framework.tests
{
    //[Parallelizable(ParallelScope.Children)] // this is for second method.
    public class EndToEndFlowParallel : DriverFactory
    {
        private LoginPage _loginPage;

        private static readonly List<LoginTestData> LoginData =
             JsonReader.ReadSection<LoginTestData>("resource/TestData.json", "login");

        [SetUp]  // Also in DriverFactory, [OneTimeSetUp] should be [Setup] to ensure it runs before each test method in parallel execution
        public void InitializePageObjects()
        {
            _loginPage = new LoginPage(Driver);
        }

        //---------------------------------------------Run all data sets of test method in parallel-----------------------------------------------------//

        [Test, TestCaseSource(nameof(LoginData))]
        [Parallelizable(ParallelScope.All)]
        public void VerifyUserSuccessfulLogin(LoginTestData data)
        {
            string userName = EnvConfig.Username;
            string password = EnvConfig.Password;
            string userType = data.UserType;
            string personType = data.PersonType;

            _loginPage.Login(userName, password, userType, personType);
        }

        [Test]
        public void VerifyUserFailedLogin()
        {
            _loginPage.Login("rahulshettyacademy", "Learning@830$3mK2", "admin", "admin");
        }

        //---------------------------------------------Run all test method in one class in parallel-----------------------------------------------------//

        //[Test, TestCaseSource(nameof(LoginData))]
        //[Parallelizable(ParallelScope.All)]
        //public void VerifyUserSuccessfulLogin(LoginTestData data)
        //{
        //    string userName = EnvConfig.Username;
        //    string password = EnvConfig.Password;
        //    string userType = data.UserType;
        //    string personType = data.PersonType;

        //    _loginPage.Login(userName, password, userType, personType);
        //}

        //[Test]
        //public void VerifyUserFailedLogin()
        //{
        //    _loginPage.Login("rahulshettyacademy", "Learning@830$3mK2", "admin", "admin");
        //}

        //---------------------------------------------Run all test files in project in parallel-----------------------------------------------------//

        // For this set [Parallelizable(ParallelScope.Self)] in class level of each test class.
    }
}