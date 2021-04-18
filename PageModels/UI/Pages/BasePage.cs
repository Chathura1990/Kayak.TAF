using Core.UI;
using PageModels.UI.Maps;
using PageModels.UI.Validators;

namespace PageModels.UI.Pages
{
    public class BasePage : Page
    {
        private readonly BasePageElementMap _map = new BasePageElementMap();

        public BasePageValidator Validate()
        {
            return new BasePageValidator(_map);
        }

        public void WaitForHeaderMainLogoToBeDisplayed(int? seconds = null)
        {
            _map.WaitForHeaderMainLogoToBeDisplayed(seconds);
        }

        public void NavigateToPage(string pageName)
        {
            _map.NavigateToPage(pageName);
        }

        public void ClickOnPrivacyPopupWindowAction(string action)
        {
            _map.PrivacyPopupWindowAction(action);
        }
    }
}