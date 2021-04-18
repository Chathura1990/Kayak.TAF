using NUnit.Framework;
using PageModels.UI.Maps;

namespace PageModels.UI.Validators
{
    public class FlightSearchPageValidator
    {
        private readonly FlightSearchPageElementMap _map;

        public FlightSearchPageValidator(FlightSearchPageElementMap map)
        {
            _map = map;
        }

        public void ErrorMessage(string errorMessage)
        {
            if (_map.CheckErrorMessage(errorMessage).WaitForToBeDisplayed())
            {
                Assert.That(_map.CheckErrorMessage(errorMessage).IsDisplayed, "Unable to check error message");
            }
        }

        public void LanguageIsSelected(string country)
        {
            if (_map.GetCountryPickerButtonAttribute() != null)
            {
                Assert.That(_map.GetCountryPickerButtonAttribute().Contains(country),"Unable to select the language");
            }
        }
    }
}