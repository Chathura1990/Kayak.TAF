using Core.Selenium.WebDriver;
using Core.UI;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;

namespace Core.Utils
{
    public static class Condition
    {
        #region Element conditions

        public static Predicate<Element> Displayed = element => element.IsDisplayed();

        public static Predicate<Element> NotDisplayed = element => !element.IsDisplayed();

        public static Predicate<Element> Disabled = element => !element.Enabled;

        public static Predicate<Element> Enabled = element => element.Enabled;

        #endregion Element conditions

        #region Browser condition

        public static Predicate<Browser> PageLoad = _ =>
        {
            RemoteWebDriver driver = TestRunner.Current.Driver;
            bool result = false;
            try
            {
                driver.SwitchTo().Window(driver.CurrentWindowHandle);
                result = driver.ExecuteScript("return (typeof($) === 'undefined') ? true : !$.active;").Equals(true);
            }
            catch (NoSuchWindowException)
            {
                driver.SwitchTo().Window(driver.WindowHandles[0]);
            }
            return result;
        };

        #endregion Browser condition
    }
}