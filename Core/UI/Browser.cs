using Core.Configurations;
using Core.Exceptions;
using Core.Selenium.WebDriver;
using Core.Utils;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Core.UI
{
    /// <summary>
    /// This class provide common browser functionality: navigating to pages, managing windows, alerts, capturing screenshots
    /// </summary>
    public class Browser
    {
        public Logger Log { get; } = Logger.Instance;
        public TestRunner Runner;
        private static ThreadLocal<Browser> _localBrowser = new ThreadLocal<Browser>();
        private bool _isValidStatement;

        protected Browser()
        {
            Runner = TestRunner.Current;
        }

        /// <summary>
        /// Get current browser instance
        /// </summary>
        /// <returns></returns>
        public static Browser Current => _localBrowser.Value ?? (_localBrowser.Value = new Browser());

        public Dictionary<string, string> GetCookies()
        {
            Dictionary<string, string> cookies = new Dictionary<string, string>();

            foreach (Cookie cookie in Runner.Driver.Manage().Cookies.AllCookies)
            {
                cookies.Add(cookie.Name, cookie.Value);
            }

            return cookies;
        }

        public void NavigateToUrl(string url)
        {
            Log.Debug("[Browser] Navigate to page [" + url + "].");
            Runner.Driver.Navigate().GoToUrl(url);
        }

        public void Back()
        {
            Log.Debug("[Browser] Go one page back.");
            Runner.Driver.Navigate().Back();
        }

        public void Forward()
        {
            Log.Debug("[Browser] Go one page forward.");
            Runner.Driver.Navigate().Forward();
        }

        public void WindowFocus()
        {
            Log.Debug("[Browser] Switch focus to browser window.");
            Runner.Driver.SwitchTo().Window(Runner.Driver.CurrentWindowHandle);
        }

        public void WindowMaximize()
        {
            Log.Debug("[Browser] Maximize browser window.");
            Runner.Driver.Manage().Window.Maximize();
        }

        public void SwitchToNewOpenedWindow()
        {
            Log.Debug("[Browser] Switch to new opened window.");

            foreach (string handle in Runner.Driver.WindowHandles)
            {
                Runner.Driver.SwitchTo().Window(handle);
            }
        }

        public void WaitForTimeout(TimeSpan timeout)
        {
            Thread.Sleep(timeout);
        }

        public static void Quit()
        {
            _localBrowser.Value = null;
        }

        public object ExecuteScript(string script, params object[] args)
        {
            object[] convertedArgs = args.Select(arg => (arg is Element) ? (arg as Element).WrappedElement : arg).ToArray();

            object result = Runner.Driver.ExecuteScript(script, convertedArgs);

            if (result is IWebElement)
            {
                return new Element(null) { WrappedElement = result as IWebElement };
            }

            if (result is List<IWebElement>)
            {
                return (result as List<IWebElement>).Select(el => new Element(null) { WrappedElement = el });
            }

            return result;
        }

        #region Alert

        private IAlert _alert;
        
        /// <summary>
        /// Usage - browser.ifIsAlertPresent().printAlertText(log, LogLevel.INFO).acceptUnexpectedAlert();
        /// </summary>
        /// <returns>Browser instance</returns>
        public Browser IfIsAlertPresent()
        {
            if (IsAlertPresent())
            {
                _isValidStatement = true;
                return this;
            }
            _isValidStatement = false;
            return this;
        }

        public bool IsAlertPresent()
        {
            try
            {
                WaitForTimeout(TimeSpan.FromSeconds(ConfigManager.Wait.DefaultTimeout));
                _alert = Runner.Driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        public string GetAlertText()
        {
            string alertText = null;
            if (IsAlertPresent())
            {
                alertText = _alert.Text;
            }
            return alertText;
        }

        /// <summary>
        /// Accepts currently active modal dialog for this particular Driver instance.
        /// </summary>
        /// <returns>A current Browser instance.</returns>
        /// <exception cref="BrowserAlertNotFoundException">if the dialog cannot be found</exception>
        public Browser AcceptAlert()
        {
            if (!_isValidStatement)
            {
                return this;
            }

            if (IsAlertPresent())
            {
                Log.Debug("[Browser] Accept browser alert popup.");
                _alert.Accept();
            }
            else
            {
                throw new BrowserAlertNotFoundException("[Browser] Expected browser alert NOT found!");
            }
            return this;
        }

        /// <summary>
        /// Accepts currently active modal dialog (if such present) for this particular Driver instance.
        /// </summary>
        /// <returns>A current Browser instance.</returns>
        public Browser AcceptUnexpectedAlert()
        {
            if (!_isValidStatement)
            {
                return this;
            }

            if (IsAlertPresent())
            {
                Log.Debug("[Browser] Accept unexpected browser alert popup.");
                _alert.Accept();
            }
            return this;
        }

        /// <summary>
        /// Closes currently active modal dialog for this particular Driver instance.
        /// </summary>
        /// <returns>A current Browser instance.</returns>
        /// <exception cref="BrowserAlertNotFoundException">if the dialog cannot be found.</exception>
        public Browser CloseAlert()
        {
            if (!_isValidStatement)
            {
                return this;
            }

            if (IsAlertPresent())
            {
                Log.Debug("[Browser] Dismiss browser alert popup.");
                _alert.Dismiss();
            }
            else
            {
                throw new BrowserAlertNotFoundException("[Browser] Expected browser alert NOT found!");
            }
            return this;
        }

        /// <summary>
        /// Closes currently active modal dialog (if such present) for this particular Driver instance.
        /// </summary>
        /// <returns>A current Browser instance.</returns>
        public Browser CloseUnexpectedAlert()
        {
            if (!_isValidStatement)
            {
                return this;
            }

            if (IsAlertPresent())
            {
                Log.Debug("[Browser] Dismiss unexpected browser alert popup.");
                _alert.Dismiss();
            }
            return this;
        }

        #endregion Alert

        public void CaptureScreen(string fileNameBase)
        {
            string artifactDirectory = ResourceUtils.GetRootPath("Screenshots");
            ResourceUtils.CreateDirectory(artifactDirectory);
            string screenshotFilePath = Path.Combine(artifactDirectory, fileNameBase + ".png");

            try
            {
                Screenshot screenshot = ((ITakesScreenshot)Runner.Driver).GetScreenshot();
                screenshot.SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);
                TestContext.AddTestAttachment(screenshotFilePath);
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("{0}. StackTrace:\n{1}", ex.Message, ex.StackTrace));
            }
        }

        public string GetCurrentUrl()
        {
            Log.Debug("[Browser] Get current URL.");
            return Runner.Driver.Url;
        }

        public void ClearAllCookies()
        {
            Runner.Driver.Manage().Cookies.DeleteAllCookies();
        }

        public void ClearLocalStorage()
        {
            Runner.Driver.ExecuteScript("localStorage.clear()");
        }

        public void ClearSessionStorage()
        {
            Runner.Driver.ExecuteScript("sessionStorage.clear()");
        }

        public void Refresh()
        {
            Runner.Driver.Navigate().Refresh();
        }
    }
}