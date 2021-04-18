using Core.Attributes;
using Core.UI;

namespace PageModels.UI.Maps
{
    public class FlightSearchResultPageElementMap : BasePageElementMap
    {
        [FindBy(".//div[@aria-label='Flight results section']")]
        public Element FlightResultSection;
        
    }
}