using OpenQA.Selenium;
using System;

namespace Core.Selenium.WebDriver
{
    public class LocatorHandler
    {
        public enum LocatorType
        {
            Xpath,
            ClassName,
            CssSelector,
            LinkText,
            PartialLinkText,
            Id,
            Name,
            TagName,
            Dom
        }

        protected Logger Log { get; } = Logger.Instance;

        private LocatorType GetLocatorType(string locator)
        {
            if (locator.StartsWith("xpath=") || locator.StartsWith("//") || locator.StartsWith("/html") || locator.StartsWith(".//") || locator.StartsWith("("))
            {
                return LocatorType.Xpath;
            }
            else if (locator.StartsWith("css=") || locator.StartsWith("#") || locator.StartsWith("."))
            {
                return LocatorType.CssSelector;
            }
            else if (locator.StartsWith("id="))
            {
                return LocatorType.Id;
            }
            else if (locator.StartsWith("name="))
            {
                return LocatorType.Name;
            }
            else if (locator.StartsWith("class="))
            {
                return LocatorType.ClassName;
            }
            else if (locator.StartsWith("tag="))
            {
                return LocatorType.TagName;
            }
            else if (locator.StartsWith("link="))
            {
                return LocatorType.LinkText;
            }
            else if (locator.StartsWith("dom"))
            {
                return LocatorType.Dom;
            }
            else
            {
                throw new NotSupportedException("Equivalent selenium By.* method for [" + locator + "] locator NOT found!");
            }
        }

        public By GetSeleniumBy(string locator)
        {
            string parsedLocator = locator;
            try
            {
                char[] splitter = new[] { '=' };

                switch (GetLocatorType(parsedLocator))
                {
                    case LocatorType.Xpath:
                        if (parsedLocator.StartsWith("xpath="))
                        {
                            parsedLocator = parsedLocator.Split(splitter, 2)[1];
                        }
                        return By.XPath(parsedLocator);

                    case LocatorType.CssSelector:
                        if (parsedLocator.StartsWith("css="))
                        {
                            parsedLocator = parsedLocator.Split(splitter, 2)[1];
                        }
                        return By.CssSelector(parsedLocator);

                    case LocatorType.Id:
                        if (parsedLocator.StartsWith("id="))
                        {
                            parsedLocator = parsedLocator.Split(splitter, 2)[1];
                        }
                        return By.Id(parsedLocator);

                    case LocatorType.Name:
                        if (parsedLocator.StartsWith("name="))
                        {
                            parsedLocator = parsedLocator.Split(splitter, 2)[1];
                        }
                        return By.Name(parsedLocator);

                    case LocatorType.ClassName:
                        if (parsedLocator.StartsWith("class="))
                        {
                            parsedLocator = parsedLocator.Split(splitter, 2)[1];
                        }
                        return By.ClassName(parsedLocator);

                    case LocatorType.TagName:
                        if (parsedLocator.StartsWith("tag="))
                        {
                            parsedLocator = parsedLocator.Split(splitter, 2)[1];
                        }
                        return By.TagName(parsedLocator);

                    case LocatorType.LinkText:
                        if (parsedLocator.StartsWith("link="))
                        {
                            parsedLocator = parsedLocator.Split(splitter, 2)[1];
                        }
                        return By.LinkText(parsedLocator);

                    default:
                        if (parsedLocator.StartsWith("dom:name="))
                        {
                            parsedLocator = parsedLocator.Split(splitter, 2)[1];
                            return By.XPath("//form[@name='" + parsedLocator + "']");
                        }
                        else if (parsedLocator.StartsWith("dom:index="))
                        {
                            parsedLocator = parsedLocator.Split(splitter, 2)[1];
                            return By.XPath("(//form)[" + parsedLocator + "]");
                        }

                        break;
                }
            }
            catch (NotImplementedException ex)
            {
                Log.Error(ex.StackTrace);
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
            return null;
        }
    }
}