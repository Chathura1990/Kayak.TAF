using Core.Configurations;
using Core.Selenium.WebDriver;
using Core.UI;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Utils
{
    public static class ElementCollectionUtils
    {
        /// <summary>
        /// Important: return filtered collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IEnumerable<T> AnyShouldBe<T>(this IEnumerable<T> collection, Predicate<T> condition, string message = null, params object[] args) where T : Element
        {
            return ShouldBe(collection, condition, message, args);
        }

        public static IEnumerable<T> ShouldBe<T>(this IEnumerable<T> collection, Predicate<T> condition, string message = null, params object[] args) where T : Element
        {
            IEnumerable<T> filtered = collection.Where(el => condition(el));
            Assert.IsTrue(filtered.Any(), message, args);
            return filtered;
        }

        public static IEnumerable<T> WaitForAny<T>(this IEnumerable<T> collection, Predicate<T> condition, TimeSpan timeout = default(TimeSpan), string message = null) where T : Element
        {
            if (timeout.Equals(default(TimeSpan)))
            {
                timeout = TimeSpan.FromSeconds(ConfigManager.Wait.DefaultTimeout);
            }

            WebDriverWait wait = new WebDriverWait(TestRunner.Current.Driver, timeout);
            wait.Until(_ => collection.Any(el => condition(el)));

            return collection;
        }

        public static IEnumerable<T> WaitForPresent<T>(this IEnumerable<T> collection, TimeSpan timeout = default(TimeSpan), string message = null) where T : Element
        {
            return WaitForAny<T>(collection, Condition.Displayed, timeout, message);
        }

        public static IEnumerable<T> WaitForVisible<T>(this IEnumerable<T> collection, TimeSpan timeout = default(TimeSpan), string message = null) where T : Element
        {
            return WaitForAny<T>(collection, Condition.Displayed, timeout, message);
        }
    }
}