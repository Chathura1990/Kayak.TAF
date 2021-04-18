using Core.Configurations;
using Core.UI;
using OpenQA.Selenium;
using System;

namespace PageModels.UI.Wrappers
{
    public class DropdownWithSearch : Element
    {
        public DropdownWithSearch(string locator, Element parent = null) : base(locator, parent)
        {
        }

        public void SelectOption(string optionValue)
        {
            if (!string.IsNullOrEmpty(optionValue))
            {
                Element input = FindElement(".//input");
                input.Click();
                input.SendKeys(Keys.Backspace);
                input.SendKeys(Keys.Backspace);
                input.Clear();
                input.SendKeys(optionValue);
                input.WaitForToBeClickable(TimeSpan.FromSeconds(ConfigManager.Wait.Timeout));
                input.SendKeys(Keys.Enter);
            }
        }
    }
}