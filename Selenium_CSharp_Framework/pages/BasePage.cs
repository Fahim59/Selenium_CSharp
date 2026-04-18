using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace CSharpSeleniumFramework.pages
{
    public class BasePage
    {
        protected IWebDriver Driver { get; }
        protected WebDriverWait Wait { get; }
        protected IJavaScriptExecutor Js { get; }

        public BasePage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            Js = (IJavaScriptExecutor)Driver;
        }

        protected void SmallWait(int milliseconds) => Thread.Sleep(milliseconds);

        public void Scroll(int xOffset, int yOffset)
        {
            SmallWait(1000);
            Js.ExecuteScript("window.scrollBy(arguments[0], arguments[1]);", xOffset, yOffset);
            SmallWait(500);
        }

        public void VerifyCurrentUrl(string expectedText)
        {
            SmallWait(2000);
            string currentUrl = Driver.Url;
            Assert.That(currentUrl.Contains(expectedText),
                $"The current URL does not contain: {expectedText}");
        }

        public IWebElement WaitForVisibility(By locator) =>
            Wait.Until(ExpectedConditions.ElementIsVisible(locator));

        public IWebElement WaitForPresence(By locator) =>
            Wait.Until(ExpectedConditions.ElementExists(locator));

        public IReadOnlyCollection<IWebElement> WaitForPresenceList(By locator) =>
            Wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));

        public void ClickElement(By locator) =>
            Wait.Until(ExpectedConditions.ElementToBeClickable(locator)).Click();

        public void ClickElementJs(By locator)
        {
            IWebElement element = Wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            Js.ExecuteScript("arguments[0].click();", element);
        }

        public void WriteSendKeys(By locator, string txt)
        {
            IWebElement element = WaitForPresence(locator);
            string existingText = element.GetAttribute("value");

            if (!string.IsNullOrEmpty(existingText))
                element.Clear();

            element.SendKeys(txt);
        }

        public void WriteJsExecutor(By locator, string txt)
        {
            IWebElement element = WaitForPresence(locator);
            string existingText = element.GetAttribute("value");

            if (!string.IsNullOrEmpty(existingText))
                Js.ExecuteScript("arguments[0].value = '';", element);

            Js.ExecuteScript("arguments[0].value = arguments[1];", element, txt);
        }

        public void ClickRadioElement(By locator, string text)
        {
            var options = WaitForPresenceList(locator);

            foreach (IWebElement option in options)
            {
                if (option.GetAttribute("value").Equals(text, StringComparison.OrdinalIgnoreCase)
                    && !option.Selected)
                {
                    Js.ExecuteScript("arguments[0].click();", option);
                }
            }
        }

        public void ClickCheckBox(By locator)
        {
            IWebElement element = Wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            if (!element.Selected)
                Js.ExecuteScript("arguments[0].click();", element);
        }

        public void SelectFromDropdown(By locator, string text)
        {
            IWebElement element = WaitForPresence(locator);
            new SelectElement(element).SelectByText(text);
        }

        public string GetText(By locator) => WaitForPresence(locator).Text;
    }
}