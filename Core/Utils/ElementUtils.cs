using Core.Configurations;
using Core.Selenium.WebDriver;
using Core.UI;
using NUnit.Framework;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace Core.Utils
{
    public static class ElementUtils
    {
        public static T Should<T>(this T element, Predicate<T> condition, string message = null, params object[] args) where T : Element
        {
            return ShouldBe(element, condition, message, args);
        }

        public static T ShouldBe<T>(this T element, Predicate<T> condition, string message = null, params object[] args) where T : Element
        {
            Assert.IsTrue(condition(element), message, args);
            return element;
        }

        public static T ShouldHaveText<T>(this T element, string text) where T : Element
        {
            string actualText = element.Text;
            Assert.IsTrue(text.Trim().Equals(actualText.Trim()), "Wrong element text. Expected:\n{0}\nActual:\n{1}", text, actualText);
            return element;
        }

        public static T ShouldContainText<T>(this T element, string text) where T : Element
        {
            string actualText = element.Text;
            Assert.IsTrue(actualText.Contains(text), "Wrong element text. Expected contains:\n{0}\nActual:\n{1}", text, actualText);
            return element;
        }

        public static T WaitFor<T>(this T element, Predicate<T> condition, TimeSpan timeout = default, string message = null) where T : Element
        {
            if (timeout.Equals(default))
            {
                timeout = TimeSpan.FromSeconds(ConfigManager.Wait.DefaultTimeout);
            }

            WebDriverWait wait = new WebDriverWait(TestRunner.Current.Driver, timeout);
            wait.Until(_ => condition(element));

            return element;
        }

        public static bool HasText(this Element element, string text)
        {
            return element.Text.Trim().Equals(text.Trim());
        }

        public static bool ContainsText(this Element element, string text)
        {
            return element.Text.Contains(text);
        }

        #region Mouse actions, JS, etc

        public static T HighLight<T>(this T element) where T : Element
        {
            element.WaitFor(Condition.Displayed);
            const string changeColorScript = "arguments[0].style.background = '{0}'";

            string oldColor = element.GetCss("background-color");

            Browser.Current.ExecuteScript(string.Format(changeColorScript, "#ff0000"), element);
            Browser.Current.ExecuteScript(string.Format(changeColorScript, oldColor), element);

            return element;
        }

        public static void HideElement<T>(this T element) where T : Element
        {
            Browser.Current.ExecuteScript("arguments[0].style.visibility='hidden'", element);
        }

        public static void JsClick<T>(this T element) where T : Element
        {
            Browser.Current.ExecuteScript("arguments[0].click();", element);
        }

        public static T MouseHover<T>(this T element) where T : Element
        {
            Actions action = new Actions(TestRunner.Current.Driver);
            Actions hover = action.MoveToElement(element.WrappedElement);
            hover.Perform();
            return element;
        }

        public static T ScrollTo<T>(this T element) where T : Element
        {
            Browser.Current.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            return element;
        }

        public static T RemoveStyles<T>(this T element) where T : Element
        {
            Browser.Current.ExecuteScript("arguments[0].removeAttribute('style')", element);
            return element;
        }

        #endregion Mouse actions, JS, etc
    }
}