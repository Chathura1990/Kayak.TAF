using Core;
using PageModels.Models;
using PageModels.UI;
using PageModels.UI.Pages;
using System;

namespace UiSteps.Steps
{
    public class FlightSearchUiSteps : BaseUiSteps
    {
        public FlightSearchUiSteps(StepsContext stepsContext) : base(stepsContext)
        {
        }

        public void IClickOnOriginPanel()
        {
            PageFactory.GetPage<FlightSearchPage>().ClickOnOriginPanel();
        }

        public void ISelectOriginFromDropdown(FlightSearchDataModel from)
        {
            PageFactory.GetPage<FlightSearchPage>().SelectOriginFromDropdown(from);
        }

        public void IClickOnDestinationPanel()
        {
            PageFactory.GetPage<FlightSearchPage>().ClickOnDestinationPanel();
        }

        public void ISelectDestinationFromDropdown(FlightSearchDataModel to)
        {
            PageFactory.GetPage<FlightSearchPage>().SelectDestinationFromDropdown(to);
        }

        public void IClickOnDepartureDateInput()
        {
            PageFactory.GetPage<FlightSearchPage>().ClickOnDepartureDatePanel();
        }

        public void IEnterDepartureDate(FlightSearchDataModel date)
        {
            PageFactory.GetPage<FlightSearchPage>().EnterDepartureDate(date);
        }

        public void IClickOnReturnDateInput()
        {
            PageFactory.GetPage<FlightSearchPage>().ClickOnReturnDateInput();
        }

        public void IEnterReturnDate(FlightSearchDataModel date)
        {
            PageFactory.GetPage<FlightSearchPage>().EnterReturnDate(date);
        }

        public void IClickSearchFlightsButton()
        {
            PageFactory.GetPage<FlightSearchPage>().ClickSearchFlightsButton();
        }

        public void IShouldSeeErrorMessage(string errorMessage)
        {
            PageFactory.GetPage<FlightSearchPage>().Validate().ErrorMessage(errorMessage);
        }

        public void IClickOnCountryPickerButton()
        {
            PageFactory.GetPage<FlightSearchPage>().ClickOnCountryPickerButton();
        }

        public void ISelectACountryLanguage(string country)
        {
            PageFactory.GetPage<FlightSearchPage>().SelectACountryLanguage(country);
        }

        public void IShouldSeeLanguageIsSelected(string country)
        {
            PageFactory.GetPage<FlightSearchPage>().Validate().LanguageIsSelected(country);
        }

        public void IFillAdditionalTravelInfoSection(FlightSearchDataModel additionalInfo)
        {
            PageFactory.GetPage<FlightSearchPage>().FillAdditionalTravelInfo(additionalInfo);
        }
    }
}