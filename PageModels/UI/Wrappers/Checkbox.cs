using Core.UI;

namespace PageModels.UI.Wrappers
{
    public class Checkbox : Element
    {
        public Checkbox(string locator, Element parent = null) : base(locator, parent)
        {
        }

        public bool IsChecked
        {
            get { return !string.IsNullOrEmpty(GetAttribute("checked")); }
        }

        public void SetState(bool state)
        {
            if (IsChecked != state)
            {
                Click();
            }
        }
    }
}