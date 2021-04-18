using Core.UI;

namespace PageModels.UI.Wrappers
{
    public class FileInput : Element
    {
        public FileInput(string locator, Element parent = null) : base(locator, parent)
        {
        }

        public void UploadFileFromPath(string path)
        {
            WrappedElement.SendKeys(path);
        }
    }
}