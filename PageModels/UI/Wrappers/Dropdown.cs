using Core.UI;
using Core.Utils;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace PageModels.UI.Wrappers
{
    public class Dropdown : Element
    {
        public Dropdown(string locator, Element parent = null) : base(locator, parent)
        {
        }

        private const string OptionLocator = ".//li[contains(text(),'{0}')]";

        public void SelectOption(string optionValue)
        {
            if (!string.IsNullOrEmpty(optionValue))
            {
                FindElement(string.Format(OptionLocator, optionValue)).Click();
            }
        }

        internal List<string> GetOptions()
        {
            List<Element> options = FindElements(".//li")
                .WaitForAny(Condition.Enabled).ToList()
                .SkipLast(2).ToList();

            List<string> result = new List<string>();

            if (options.Count > 0)
            {
                foreach (Element el in options)
                {
                    el.ScrollTo().MouseHover();
                    result.Add(el.Text.Trim());
                }
                return result;
            }
            else
            {
                throw new NoSuchElementException("Dropdown list options were not found");
            }
        }
    }
}