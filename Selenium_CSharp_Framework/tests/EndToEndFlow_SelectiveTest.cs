using Selenium_CSharp_Framework.factory;
using Selenium_CSharp_Framework.pages;
using Selenium_CSharp_Framework.utilities;

namespace Selenium_CSharp_Framework.tests
{
    public class EndToEndFlow_SelectiveTest : DriverFactory
    {
        private LoginPage _loginPage;

        [OneTimeSetUp]
        public void InitializePageObjects()
        {
            _loginPage = new LoginPage(Driver);
        }

        [Test, Category("SmokeTest")]
        public void VerifyUserSuccessfulLogin()
        {
            string userName = EnvConfig.Username;
            string password = EnvConfig.Password;

            _loginPage.Login(userName, password, "admin", "Consultant");
        }

        [Test, Category("RegressionTest")]
        public void VerifyUserFailedLogin()
        {
            string userName = EnvConfig.Username;
            string password = EnvConfig.Password;

            _loginPage.Login(userName, password, "admin", "admin");
        }

        //Open Termina;l and run the following command to execute only the SmokeTest category:
        //cd CSharpSeleniumFramework
        //dotnet test CSharpSeleniumFramework.csproj
        //dotnet test --filter "Category=RegressionTest"
        //dotnet test CSharpSeleniumFramework.csproj --filter "Category=SmokeTest"
    }
}