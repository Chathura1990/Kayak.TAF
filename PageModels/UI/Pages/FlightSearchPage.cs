using System;
using PageModels.Models;
using PageModels.UI.Maps;
using PageModels.UI.Validators;

namespace PageModels.UI.Pages
{
    public class FlightSearchPage: BasePage
    {
        private readonly Lazy<FlightSearchPageElementMap> _map = new Lazy<FlightSearchPageElementMap>();

        public new FlightSearchPageValidator Validate()
        {
            return new FlightSearchPageValidator(_map.Value);
        }

        public void ClickOnOriginPanel()
        {
            _map.Value.FlightOriginPanel.Click();
        }

        public void SelectOriginFromDropdown(FlightSearchDataModel flightSearchDataModel)
        {
            _map.Value.FlightOriginInput.SelectOption(flightSearchDataModel.Origin);
        }

        public void ClickOnDestinationPanel()
        {
            _map.Value.FlightDestinationPanel.Click();
        }

        public void SelectDestinationFromDropdown(FlightSearchDataModel flightSearchDataModel)
        {
            _map.Value.SelectDestinationOption(flightSearchDataModel.Destination);
        }

        public void ClickOnDepartureDatePanel()
        {
            _map.Value.DepartureDatePanel.Click();
        }

        public void EnterDepartureDate(FlightSearchDataModel flightSearchDataModel)
        {
            _map.Value.EnterDepartureDate(flightSearchDataModel.DateFrom);
        }

        public void ClickOnReturnDateInput()
        {
            _map.Value.ReturnDatePanel.Click();
        }

        public void EnterReturnDate(FlightSearchDataModel flightSearchDataModel)
        {
            _map.Value.EnterReturnDate(flightSearchDataModel.DateTo);
        }

        public void ClickSearchFlightsButton()
        {
            _map.Value.SearchFlightsButton.Click();
        }

        public void ClickOnCountryPickerButton()
        {
            _map.Value.ClickCountryPicker();
        }

        public void SelectACountryLanguage(string country)
        {
            _map.Value.SelectACountryLanguage(country);
        }

        public void FillAdditionalTravelInfo(FlightSearchDataModel additionalInfo)
        {
            _map.Value.FillAdditionalTravelInfo(additionalInfo);
        }
    }
}