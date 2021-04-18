using System;
using PageModels.UI.Maps;
using PageModels.UI.Validators;

namespace PageModels.UI.Pages
{
    public class FlightSearchResultPage : BasePage
    {
        private readonly Lazy<FlightSearchResultPageElementMap> _map = new Lazy<FlightSearchResultPageElementMap>();

        public new FlightSearchResultPageValidator Validate()
        {
            return new FlightSearchResultPageValidator(_map.Value);
        }
        
    }
}