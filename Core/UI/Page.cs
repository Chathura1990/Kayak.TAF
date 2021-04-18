using Core.Exceptions;
using Core.Utils;
using OpenQA.Selenium;

namespace Core.UI
{
    public class Page
    {
        protected internal Browser Browser;

        public Page()
        {
            Browser = Browser.Current;
        }

        #region Page interaction methods

        public string GetTitle()
        {
            return Browser.Runner.Driver.Title;
        }

        public string GetUrl()
        {
            return Browser.Runner.Driver.Url;
        }

        public void Refresh()
        {
            Browser.Log.Info("[Page] Refresh page.");
            Browser.Runner.Driver.Navigate().Refresh();
        }

        public bool ContainsText(string text)
        {
            return Browser.Runner.Driver.PageSource.Contains(text);
        }

        #endregion Page interaction methods

        #region Frames, windows

        public void SelectWindow(string iframeName)
        {
            Browser.Runner.Driver.SwitchTo().Window(iframeName);
        }

        public void SwitchToFrame(Element iframe)
        {
            Browser.Runner.Driver.SwitchTo().Frame(iframe.Locator);
        }

        public void SwitchToFrame(int iframeIndex)
        {
            Browser.Runner.Driver.SwitchTo().Frame(iframeIndex);
        }

        public void SwitchToFrame(string iframeName)
        {
            Browser.Log.Debug("[Page] Switch interaction to frame [" + iframeName + "].");
            try
            {
                Browser.Runner.Driver.SwitchTo().Frame(iframeName);
            }
            catch (NoSuchFrameException e)
            {
                Browser.Log.Error(e);
                throw new PageContentNotFoundException("[Page] Iframe block with name [" + iframeName + "] NOT found!");
            }
        }

        public void SwitchToDefaultContent()
        {
            Browser.Log.Debug("[Page] Switch interaction to default content.");
            Browser.Runner.Driver.SwitchTo().DefaultContent();
        }

        #endregion Frames, windows
    }
}