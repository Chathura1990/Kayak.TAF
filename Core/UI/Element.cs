using Core.Configurations;
using Core.Exceptions;
using Core.Selenium.WebDriver;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace Core.UI
{
    public class Element
    {
        public Element(string locator, Element parent = null)
        {
            Parent = parent;
            Locator = locator;
            Runner = TestRunner.Current;
        }

        protected internal TestRunner Runner;
        protected Logger Log { get; } = Logger.Instance;

        protected LocatorHandler Lh = new LocatorHandler();
        protected Element Parent { get; set; }
        public string Locator { get; set; }

        protected internal bool IsValidStatement = true;

        private ISearchContext GetSearchContext()
        {
            if (Parent != null)
            {
                return Parent.WrappedElement;
            }
            return Runner.Driver;
        }

        protected internal void Invalidate()
        {
            if (Locator != null)
            {
                WrappedElement = null;
            }
            else
            {
                throw new InvalidateElementException("Element was invalidated, cannot re-search it as it has no locator");
            }
        }

        protected void InvalidateParent()
        {
            Parent?.Invalidate();
        }

        private IWebElement _wrappedElement;

        protected internal IWebElement WrappedElement
        {
            get
            {
                if (_wrappedElement == null)
                {
                    try
                    {
                        _wrappedElement = GetSearchContext().FindElement(Lh.GetSeleniumBy(Locator));
                    }
                    catch (StaleElementReferenceException)
                    {
                        InvalidateParent();
                        _wrappedElement = GetSearchContext().FindElement(Lh.GetSeleniumBy(Locator));
                    }
                }
                return _wrappedElement;
            }
            set => _wrappedElement = value;
        }

        #region Find elements

        public T FindElement<T>(string locator) where T : Element
        {
            return (T)Activator.CreateInstance(typeof(T), locator, this);
        }

        public ElementCollection<T> FindElements<T>(string locator) where T : Element
        {
            return new ElementCollection<T>(locator, this);
        }

        public Element FindElement(string locator)
        {
            return FindElement<Element>(locator);
        }

        public ElementCollection FindElements(string locator)
        {
            return new ElementCollection(locator, this);
        }

        #endregion Find elements

        public bool IsPresent()
        {
            try
            {
                if (GetSearchContext().FindElements(Lh.GetSeleniumBy(Locator)).Count > 0)
                {
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
            return false;
        }

        public bool IsDisplayed()
        {
            try
            {
                if (WrappedElement.Displayed)
                {
                    return true;
                }
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
            return false;
        }

        public bool IsDisabled()
        {
            try
            {
                return !WrappedElement.Enabled;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (StaleElementReferenceException)
            {
                return false;
            }
        }

        #region Waits

        public bool WaitForToBeDisplayed(TimeSpan timeout = default(TimeSpan))
        {
            timeout = timeout.Equals(default(TimeSpan)) ? TimeSpan.FromSeconds(ConfigManager.Wait.DefaultTimeout) : timeout;
            WebDriverWait wait = new WebDriverWait(Runner.Driver, timeout)
            {
                PollingInterval = TimeSpan.FromSeconds(ConfigManager.Wait.PollingInterval)
            };

            try
            {
                return wait.Until(_ =>
                {
                    try
                    {
                        return WrappedElement?.Displayed == true;
                    }
                    catch (NoSuchElementException e)
                    {
                        Log.Warn(e.Message);
                        return false;
                    }
                    catch (StaleElementReferenceException e)
                    {
                        Invalidate();
                        Log.Info(e.Message);
                        return false;
                    }
                });
            }
            catch (WebDriverTimeoutException e)
            {
                Log.Error(e.StackTrace);
                throw new ElementWaitTimeoutException($"[Element] Element located by [{Locator}] is NOT present after {timeout.TotalSeconds} second(s) expectation.");
            }
        }

        public bool WaitForToBeSelected(TimeSpan timeout = default(TimeSpan))
        {
            timeout = timeout.Equals(default(TimeSpan)) ? TimeSpan.FromSeconds(ConfigManager.Wait.DefaultTimeout) : timeout;
            WebDriverWait wait = new WebDriverWait(Runner.Driver, timeout)
            {
                PollingInterval = TimeSpan.FromSeconds(ConfigManager.Wait.PollingInterval)
            };

            try
            {
                return wait.Until(_ =>
                {
                    try
                    {
                        return WrappedElement?.Selected == true;
                    }
                    catch (NoSuchElementException e)
                    {
                        Log.Warn(e.Message);
                        return false;
                    }
                    catch (StaleElementReferenceException e)
                    {
                        Invalidate();
                        Log.Info(e.Message);
                        return false;
                    }
                });
            }
            catch (WebDriverTimeoutException e)
            {
                Log.Error(e.StackTrace);
                throw new ElementWaitTimeoutException($"[Element] Element located by [{Locator}] is NOT present after {timeout.TotalSeconds} second(s) expectation.");
            }
        }


        public bool WaitForToBeInvisible(TimeSpan timeout = default(TimeSpan))
        {
            timeout = timeout.Equals(default(TimeSpan)) ? TimeSpan.FromSeconds(ConfigManager.Wait.DefaultTimeout) : timeout;
            WebDriverWait wait = new WebDriverWait(Runner.Driver, timeout)
            {
                PollingInterval = TimeSpan.FromSeconds(ConfigManager.Wait.PollingInterval)
            };

            try
            {
                return wait.Until(_ =>
                {
                    try
                    {
                        return !WrappedElement.Displayed;
                    }
                    catch (NoSuchElementException)
                    {
                        return true;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return true;
                    }
                });
            }
            catch (WebDriverTimeoutException e)
            {
                Log.Error(e.StackTrace);
                throw new ElementWaitTimeoutException($"[Element] Element located by [{Locator}] is displayed after {timeout.TotalSeconds} second(s) expectation.");
            }
        }

        public bool WaitForToBeClickable(TimeSpan timeout = default(TimeSpan))
        {
            timeout = timeout.Equals(default(TimeSpan)) ? TimeSpan.FromSeconds(ConfigManager.Wait.DefaultTimeout) : timeout;
            WebDriverWait wait = new WebDriverWait(Runner.Driver, timeout)
            {
                PollingInterval = TimeSpan.FromSeconds(ConfigManager.Wait.PollingInterval)
            };

            try
            {
                return wait.Until(_ =>
                {
                    try
                    {
                        return WrappedElement?.Displayed == true && WrappedElement.Enabled;
                    }
                    catch (NoSuchElementException e)
                    {
                        Log.Warn(e.Message);
                        return false;
                    }
                    catch (StaleElementReferenceException e)
                    {
                        Log.Warn(e.Message);
                        return false;
                    }
                });
            }
            catch (WebDriverTimeoutException e)
            {
                Log.Error(e.StackTrace);
                throw new ElementWaitTimeoutException($"[Element] Element located by [{Locator}] is not clickable after {timeout.TotalSeconds} second(s) expectation.");
            }
        }

        #endregion Waits

        public void Click(bool wait = true)
        {
            if (wait)
            {
                WaitForToBeClickable();
            }

            WaitForLoadingMaskToDisappear();

            WrappedElement.Click();
        }

        private void WaitForLoadingMaskToDisappear()
        {
            try
            {
                IWebElement mask = Runner.Driver.FindElementByXPath(".//div[contains(@class,'el-loading-mask')]");
                WebDriverWait webDriverWait = new WebDriverWait(Runner.Driver, TimeSpan.FromSeconds(ConfigManager.Wait.DefaultTimeout))
                {
                    PollingInterval = TimeSpan.FromSeconds(ConfigManager.Wait.PollingInterval)
                };
                webDriverWait.Until(_ => !mask.Displayed);
            }
            catch (NoSuchElementException)
            {
            }
            catch (StaleElementReferenceException)
            {
            }
        }

        public void DoubleClick()
        {
            Actions builder = new Actions(Runner.Driver);
            builder.DoubleClick(WrappedElement).Build().Perform();
        }

        public void MultipleClick()
        {
            Runner.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            for (int i = 0; i < 3; i++)
            {
                Click();
            }
        }

        #region Actions

        public string GetValue()
        {
            return WrappedElement.GetAttribute("value");
        }

        public virtual void SendKeys(string value)
        {
            WaitForToBeDisplayed();
            WrappedElement.SendKeys(value);
        }

        public virtual void Clear()
        {
            WrappedElement.Clear();
        }

        public virtual void SetValue(string value)
        {
            if (value != null)
            {
                WaitForToBeDisplayed();
                WrappedElement.Clear();
                WrappedElement.SendKeys(value);
            }
        }

        public virtual void SetValueOneByOne(string value)
        {
            if (value != null)
            {
                WaitForToBeDisplayed();
                WrappedElement.Clear();
                for (int i = 0; i < value.Length; i++)
                {
                    WrappedElement.SendKeys(value[i].ToString());
                }
            }
        }

        public string GetAttribute(string attribute)
        {
            return WrappedElement.GetAttribute(attribute);
        }

        public string GetCss(string name)
        {
            return WrappedElement.GetCssValue(name);
        }

        #endregion Actions

        #region Properties

        public string TagName => WrappedElement.TagName;

        public string Text
        {
            get
            {
                WaitForToBeDisplayed();
                return WrappedElement.Text;
            }
        }

        public string NodeText
        {
            get
            {
                return WrappedElement.Text;
            }
        }

        /// <summary>
        /// Use this method, if element is not visible
        /// </summary>
        public string InnerText => Browser.Current.ExecuteScript("return arguments[0].innerHTML", this).ToString();

        public bool Enabled => WrappedElement.Enabled;

        #endregion Properties
    }
}