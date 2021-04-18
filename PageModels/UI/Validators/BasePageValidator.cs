using PageModels.UI.Maps;

namespace PageModels.UI.Validators
{
    public class BasePageValidator
    {
        private readonly BasePageElementMap _map;

        public BasePageValidator(BasePageElementMap map)
        {
            _map = map;
        }
    }
}