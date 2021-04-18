using Core.Configurations;
using Core.Selenium.Browser;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Core.Selenium.WebDriver
{
    /// <summary>
    /// Instantiates and closes browser(s). Provides access to WebDriver for friend classes.
    /// </summary>
    public class TestRunner
    {
        public Logger Log { get; } = Logger.Instance;
        private static ThreadLocal<TestRunner> _localRunner = new ThreadLocal<TestRunner>();
        private RemoteWebDriver _driver;
        protected BrowserType BrowserType;
        protected bool DebugMode;

        public TestRunner(BrowserType browserType, bool debugMode = false)
        {
            BrowserType = browserType;
            DebugMode = debugMode;
            _localRunner.Value = this;
        }

        public static TestRunner Current => _localRunner.Value ?? throw new InvalidOperationException("TestRunner hasn't been initialized yet");

        protected internal RemoteWebDriver Driver
        {
            get => _driver ?? (_driver = InitializeDriver(BrowserType));
            set => _driver = value;
        }

        public void CloseBrowsers()
        {
            try
            {
                UI.Browser.Quit();
                Driver.Quit();
                Driver = null;
            }
            catch (Exception ex)
            {
                Log.Error($"Exception caught while closing browser: {ex.Message}");
                Log.Error($"Exception StackTrack: {ex.StackTrace}");
                Log.Error($"InnerException: {ex.InnerException?.Message}");
                Log.Error($"InnerException Exception StackTrack: {ex.InnerException?.StackTrace}");
            }
        }

        private RemoteWebDriver InitializeDriver(BrowserType browserType)
        {
            Log.Info($"Browser type: {browserType}");

            RemoteWebDriver driver;

            string pathToDriverBinary = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            switch (browserType)
            {
                case BrowserType.Chrome:
                    {
                        ChromeOptions chromeOptions = new ChromeOptions();

                        if (ConfigManager.Browser.Headless)
                        {
                            chromeOptions.AddArguments("--headless");
                            chromeOptions.AddArguments("--no-sandbox");
                        } 
                        
                        driver = new ChromeDriver(pathToDriverBinary, chromeOptions);

                        break;
                    }
                
                //Here we can implement another cases for other browsers

                default:
                    {
                        throw new NotImplementedException(string.Format("Launch for '{0}' is not implemented yet", browserType));
                    }
            }

            if (!ConfigManager.Browser.Headless)
            {
                driver.Manage().Window.Maximize();
            }
            else
            {
                driver.Manage().Window.Size = new Size(ConfigManager.Browser.WindowWidth, ConfigManager.Browser.WindowHeight);
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);

            driver.FileDetector = new LocalFileDetector();
            return driver;
        }
    }
}