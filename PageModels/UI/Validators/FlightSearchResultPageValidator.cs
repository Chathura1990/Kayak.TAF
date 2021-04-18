using NUnit.Framework;
using PageModels.UI.Maps;

namespace PageModels.UI.Validators
{
    public class FlightSearchResultPageValidator
    {
        private readonly FlightSearchResultPageElementMap _map;

        public FlightSearchResultPageValidator(FlightSearchResultPageElementMap map)
        {
            _map = map;
        }
        
        public void FlightResultSection()
        {
            if (_map.FlightResultSection.WaitForToBeDisplayed())
            {
                Assert.That(_map.FlightResultSection.IsDisplayed, "There were no flight result section");
            }
        }
    }
}