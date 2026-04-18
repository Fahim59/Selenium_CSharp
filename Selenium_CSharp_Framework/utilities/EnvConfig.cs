namespace Selenium_CSharp_Framework.utilities
{
    public class EnvConfig
    {
        public string Browser { get; private set; }
        public string BaseUrl { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string UploadPath { get; private set; }
        public string ReportPath { get; private set; }

        private static EnvConfig _instance;
        public static EnvConfig Instance => _instance ??= Load();

        private static EnvConfig Load()
        {
            DotNetEnv.Env.Load();

            string browser = Environment.GetEnvironmentVariable("BROWSER") ?? "chrome";
            string env = Environment.GetEnvironmentVariable("ENV") ?? "dev";
            string reportPath = Environment.GetEnvironmentVariable("REPORT_PATH") ?? "Reports/TestReport.html";

            EnvConfig config = env.ToLower() switch
            {
                "dev" => new EnvConfig
                {
                    BaseUrl = Environment.GetEnvironmentVariable("DEV_URL") ?? "",
                    Username = Environment.GetEnvironmentVariable("DEV_USER") ?? "",
                    Password = Environment.GetEnvironmentVariable("DEV_PASS") ?? "",
                    UploadPath = "resources/Pictures",
                },
                "staging" => new EnvConfig
                {
                    BaseUrl = Environment.GetEnvironmentVariable("STAGE_URL") ?? "",
                    Username = Environment.GetEnvironmentVariable("STAGE_USER") ?? "",
                    Password = Environment.GetEnvironmentVariable("STAGE_PASS") ?? "",
                    UploadPath = "resources/Pictures",
                },
                "prod" => new EnvConfig
                {
                    BaseUrl = Environment.GetEnvironmentVariable("PROD_URL") ?? "",
                    Username = Environment.GetEnvironmentVariable("PROD_USER") ?? "",
                    Password = Environment.GetEnvironmentVariable("PROD_PASS") ?? "",
                    UploadPath = "resources/Pictures",
                },
                _ => throw new ArgumentException($"Unsupported ENV: {env}")
            };

            config.Browser = browser;
            config.ReportPath = reportPath;
            return config;
        }
    }
}