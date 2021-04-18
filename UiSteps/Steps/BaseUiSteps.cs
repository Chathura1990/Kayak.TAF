using Core;
using Core.UI;
using PageModels.UI;
using PageModels.UI.Pages;
using System;

namespace UiSteps.Steps
{
    public class BaseUiSteps
    {
        protected readonly StepsContext stepsContext;

        public BaseUiSteps(StepsContext stepsContext)
        {
            this.stepsContext = stepsContext;
        }

        protected Logger Log { get; } = Logger.Instance;
        
        public void IWaitForHeaderMainLogoToBeDisplayed()
        {
            PageFactory.GetPage<BasePage>().WaitForHeaderMainLogoToBeDisplayed();
        }

        public void RefreshPage()
        {
            Browser.Current.Refresh();
        }

        public void IClickOnPrivacyPopupWindowAction(string action)
        {
            PageFactory.GetPage<BasePage>().ClickOnPrivacyPopupWindowAction(action);
        }

        public void INavigateToPage(string page)
        {
            PageFactory.GetPage<BasePage>().NavigateToPage(page);
        }
    }
}