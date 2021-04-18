using PageModels.UI.Pages;

namespace PageModels.UI
{
    public static class PageFactory
    {
        public static TPage GetPage<TPage>() where TPage : BasePage, new()
        {
            return new TPage();
        }
    }
}