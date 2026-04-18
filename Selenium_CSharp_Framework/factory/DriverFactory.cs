using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using CSharpSeleniumFramework.utilities;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using WebDriverManager.DriverConfigs.Impl;

namespace CSharpSeleniumFramework.factory
{
    public abstract class DriverFactory
    {
        private static ExtentReports _extent;
        private static readonly ThreadLocal<ExtentTest> _test = new();
        protected ExtentTest Test => _test.Value;

        private static readonly ThreadLocal<IWebDriver> _driver = new();

        protected IWebDriver Driver
        {
            get => _driver.Value
                   ?? throw new InvalidOperationException("Driver is null. Did OneTimeSetup run?");
            private set => _driver.Value = value;
        }
        protected EnvConfig EnvConfig { get; private set; }

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            EnvConfig = EnvConfig.Instance;

            lock (typeof(DriverFactory))
            {
                if (_extent == null)
                {
                    string binDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    var projectDir = Directory.GetParent(binDirectory)?.Parent?.Parent?.Parent;

                    if (projectDir == null)
                        throw new DirectoryNotFoundException("Could not determine project root directory.");

                    string fullReportPath = Path.Combine(projectDir.FullName, "reports", "TestReport.html");

                    string reportDir = Path.GetDirectoryName(fullReportPath)!;
                    if (!Directory.Exists(reportDir))
                        Directory.CreateDirectory(reportDir);

                    var htmlReporter = new ExtentSparkReporter(fullReportPath);
                    htmlReporter.Config.DocumentTitle = "Automation Test Report";
                    htmlReporter.Config.ReportName = "Regression Suite";

                    _extent = new ExtentReports();
                    _extent.AttachReporter(htmlReporter);
                    _extent.AddSystemInfo("Environment", EnvConfig.BaseUrl);
                    _extent.AddSystemInfo("Browser", EnvConfig.Browser);
                    _extent.AddSystemInfo("OS", Environment.OSVersion.ToString());

                    TestContext.Progress.WriteLine($"REPORT IS BEING CREATED AT: {fullReportPath}");
                }
            }
            InitBrowser(EnvConfig.Browser);
            Driver.Url = EnvConfig.BaseUrl;
        }

        [SetUp]
        public void SetupTest()
        {
            _test.Value = _extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        private void InitBrowser(string browserName)
        {
            switch ((browserName ?? string.Empty).ToLowerInvariant())
            {
                case "chrome":
                    ChromeOptions options = new ChromeOptions();

                    options.AddExcludedArgument("enable-automation");
                    options.AddAdditionalChromeOption("useAutomationExtension", false);
                    options.AddArgument("--disable-notifications");

                    string downloadPath = "C:\\Your\\Download\\Path";
                    options.AddUserProfilePreference("download.default_directory", downloadPath);
                    options.AddUserProfilePreference("profile.default_content_settings.popups", 0);

                    options.AddUserProfilePreference("credentials_enable_service", false);         //prevent pop up of save password window
                    options.AddUserProfilePreference("profile.password_manager_enabled", false);  //prevent pop up of save password window

                    options.AddUserProfilePreference("autofill.profile_enabled", false);         //prevent autofill for addresses and profiles
                    options.AddUserProfilePreference("autofill.address_enabled", false);        //prevent address autofill

                    options.AddUserProfilePreference("autofill.credit_card_enabled", false);   //prevent the Save Card popup

                    new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
                    Driver = new ChromeDriver(options);
                    break;

                case "firefox":
                    FirefoxOptions firefoxOptions = new FirefoxOptions();

                    firefoxOptions.SetPreference("dom.webnotifications.enabled", false);

                    firefoxOptions.SetPreference("browser.download.dir", "/path/to/download");
                    firefoxOptions.SetPreference("browser.download.folderList", 2);
                    firefoxOptions.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/pdf,application/octet-stream");

                    firefoxOptions.SetPreference("signon.rememberSignons", false);     //prevent pop up of save password window
                    firefoxOptions.SetPreference("signon.autofillForms", false);      //prevent pop up of save password window

                    new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
                    Driver = new FirefoxDriver(firefoxOptions);
                    break;

                default:
                    throw new ArgumentException($"Unsupported browser: {browserName}");
            }

            Driver.Manage().Window.Maximize();
        }

        private string CaptureScreenshot()
        {
            return ((ITakesScreenshot)Driver).GetScreenshot().AsBase64EncodedString;
        }

        [TearDown]
        public void TearDownTest()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var message = TestContext.CurrentContext.Result.Message ?? string.Empty;
            var testName = TestContext.CurrentContext.Test.Name;

            switch (status)
            {
                case TestStatus.Failed:
                    var stackTrace = TestContext.CurrentContext.Result.StackTrace;
                    _test.Value
                         .Fail($"<b>Test failed:</b> {message}<br><pre>{stackTrace}</pre>")
                         .AddScreenCaptureFromBase64String(CaptureScreenshot(), testName);
                    break;

                case TestStatus.Passed:
                    _test.Value.Pass("Test passed successfully.");
                    break;

                case TestStatus.Skipped:
                    _test.Value.Skip("Test was skipped.");
                    break;

                default:
                    _test.Value.Warning($"Test ended with status: {status}");
                    break;
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            try
            {
                Driver?.Quit();
                Driver?.Dispose();
                _driver.Value = null;
            }
            catch
            {
                // ignore driver quit errors
            }
            finally
            {
                lock (typeof(DriverFactory))
                {
                    _extent?.Flush();
                }
            }
        }
    }
}