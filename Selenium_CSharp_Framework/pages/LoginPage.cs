using OpenQA.Selenium;

namespace CSharpSeleniumFramework.pages
{
    internal class LoginPage(IWebDriver driver) : BasePage(driver)
    {
        private readonly By _userField = By.Id("username");
        private readonly By _passwordField = By.Id("password");
        private readonly By _userTypeField = By.CssSelector("input[type='radio']");
        private readonly By _personTypeField = By.XPath("//select[@class='form-control']");
        private readonly By _agreeField = By.Id("terms");
        private readonly By _signInButton = By.Id("signInBtn");

        private readonly By _checkOutButton = By.CssSelector(".nav-link.btn.btn-primary");

        public LoginPage EnterEmail(string email)
        {
            WriteSendKeys(_userField, email);
            return this;
        }
        public LoginPage EnterPassword(string password)
        {
            WriteSendKeys(_passwordField, password);
            return this;
        }
        public LoginPage SelectUserType(string userType)
        {
            ClickRadioElement(_userTypeField, userType);
            return this;
        }
        public LoginPage SelectPersonType(string personType)
        {
            SelectFromDropdown(_personTypeField, personType);
            return this;
        }
        public LoginPage AgreeToTerms()
        {
            ClickCheckBox(_agreeField);
            return this;
        }
        public void ClickSignIn()
        {
            ClickElement(_signInButton);
        }

        public void Login(string email, string password, string userType, string personType)
        {
            EnterEmail(email)
                .EnterPassword(password)
                .SelectUserType(userType)
                .SelectPersonType(personType)
                .AgreeToTerms()
                .ClickSignIn();
        }

        public void VisibilityOfElements()
        {
            WaitForVisibility(_checkOutButton);
            SmallWait(5000);
        }
    }
}