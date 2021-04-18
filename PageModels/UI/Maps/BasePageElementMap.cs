using System;
using Core;
using Core.Attributes;
using Core.Exceptions;
using Core.UI;

namespace PageModels.UI.Maps
{
    public class BasePageElementMap : Block
    {
        [FindBy(".//div[@class='sign-in-nav-link']")]
        public Element SignIn;

        [FindBy(".//div[contains(@class,'HeaderMainLogo')]")]
        public Element HeaderMainLogo;

        private const string NavContainerLinks = ".//div[@class='ZRVS']/a[@href='/{0}']";

        private const string PrivacyPopupWindow = "//button[@aria-label='{0}']";

        public BasePageElementMap(string locator = null) : base(locator ?? "//body")
        {
            InitializeFields();
            WaitLoadIndicatorsFields();
        }

        public void WaitForHeaderMainLogoToBeDisplayed(int? seconds = null)
        {
            int timeoutSeconds = 1;

            if (seconds != null)
            {
                timeoutSeconds = (int) seconds;
            }

            try
            {
                HeaderMainLogo.WaitForToBeDisplayed(TimeSpan.FromSeconds(timeoutSeconds));
            }
            catch (ElementWaitTimeoutException)
            {
            }
        }

        public void PrivacyPopupWindowAction(string action)
        {
            WaitForHeaderMainLogoToBeDisplayed();
            FindElement(string.Format(PrivacyPopupWindow, action)).Click();
        }

        public void NavigateToPage(string uri)
        {
            WaitForHeaderMainLogoToBeDisplayed();
            FindElement(string.Format(NavContainerLinks, uri)).Click();
        }
    }
}