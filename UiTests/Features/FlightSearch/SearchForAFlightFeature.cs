using Core;
using Core.Extensions;
using NUnit.Framework;
using PageModels.App.StaticData;
using PageModels.App.StaticData.AirportCodes;
using PageModels.Models;
using TestStack.BDDfy;

namespace UiTests.Features.FlightSearch
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class SearchForAFlightFeature : UiTestHelper
    {
        public SearchForAFlightFeature()
        {
            Logger.Debug($"[Feature] {GetType()}");
        }

        [Test]
        [Category(TestCategories.Smoke)]
        [Category(TestCategories.Medium)]
        public void VerifyUserIsAbleToSearchForAFlightWithInvalidInfo()
        {
            FlightSearchDataModel flightSearchDataModel = new FlightSearchDataModel
            {
                Origin = "Test",
                Destination = AirportsStartsWithC.CapeTownCapeTownInternationalAirport,
                DateFrom = LocalToday.AddMonths(1).AsLocalDate(),
                DateTo = LocalToday.AddMonths(2).AddDays(6).AsLocalDate()
            };

            const string errorMessage = "From";

            That.Given(_ => BaseUiSteps.IClickOnPrivacyPopupWindowAction(PrivacyPopupWindowActions.Accept))
                .And(_ => BaseUiSteps.INavigateToPage(PageUriFields.Flights))
                .When(_ => FlightSearchUiSteps.IClickOnOriginPanel())
                .And(_ => FlightSearchUiSteps.ISelectOriginFromDropdown(flightSearchDataModel))
                .And(_ => FlightSearchUiSteps.IClickOnDestinationPanel())
                .And(_ => FlightSearchUiSteps.ISelectDestinationFromDropdown(flightSearchDataModel))
                .And(_ => FlightSearchUiSteps.IClickOnDepartureDateInput())
                .And(_ => FlightSearchUiSteps.IEnterDepartureDate(flightSearchDataModel))
                .And(_ => FlightSearchUiSteps.IClickOnReturnDateInput())
                .And(_ => FlightSearchUiSteps.IEnterReturnDate(flightSearchDataModel))
                .And(_ => FlightSearchUiSteps.IClickSearchFlightsButton())
                .Then(_ => FlightSearchUiSteps.IShouldSeeErrorMessage(errorMessage))
                .BDDfy("Verify user is able to search for a flight with invalid info");
        }

        [Test]
        [Category(TestCategories.Smoke)]
        [Category(TestCategories.High)]
        public void VerifyUserIsAbleToSearchForAFlightWithValidInfo()
        {
            FlightSearchDataModel flightSearchDataModel = new FlightSearchDataModel
            {
                Origin = AirportsStartsWithA.Abadan,
                Destination = AirportsStartsWithC.ColomboBandaranaikeInternationalAirport,
                DateFrom = LocalToday.AddMonths(1).AsLocalDate(),
                DateTo = LocalToday.AddMonths(4).AddDays(10).AsLocalDate()
            };

            That.Given(_ => BaseUiSteps.IClickOnPrivacyPopupWindowAction(PrivacyPopupWindowActions.Accept))
                .And(_ => BaseUiSteps.INavigateToPage(PageUriFields.Flights))
                .When(_ => FlightSearchUiSteps.IClickOnOriginPanel())
                .And(_ => FlightSearchUiSteps.ISelectOriginFromDropdown(flightSearchDataModel))
                .And(_ => FlightSearchUiSteps.IClickOnDestinationPanel())
                .And(_=> FlightSearchUiSteps.ISelectDestinationFromDropdown(flightSearchDataModel))
                .And(_=> FlightSearchUiSteps.IClickOnDepartureDateInput())
                .And(_=> FlightSearchUiSteps.IEnterDepartureDate(flightSearchDataModel))
                .And(_=> FlightSearchUiSteps.IClickOnReturnDateInput())
                .And(_=> FlightSearchUiSteps.IEnterReturnDate(flightSearchDataModel))
                .And(_=> FlightSearchUiSteps.IClickSearchFlightsButton())
                .Then(_ => FlightSearchResultUiSteps.IShouldSeeTheFlightResultSection())
                .BDDfy("Verify user is able to search for a flight with valid info");
        }

        [Test]
        [Category(TestCategories.Smoke)]
        [Category(TestCategories.High)]
        public void VerifyUserIsAbleToSearchForAFlightWithAdditionalValidInfo()
        {
            FlightSearchDataModel flightSearchDataModel = new FlightSearchDataModel
            {
                Origin = AirportsStartsWithA.Abadan,
                Destination = AirportsStartsWithC.ColomboBandaranaikeInternationalAirport,
                DateFrom = LocalToday.AddMonths(2).AsLocalDate(),
                DateTo = LocalToday.AddMonths(3).AddDays(10).AsLocalDate(),
                TripType = TripTypes.Return,
                Travelers = new TravelersAboveForm
                {
                    Adults = 2,
                    Children = 1,
                    InfantOnLap = 1
                },
                CabinClass = CabinClasses.First,
                Baggage = new Baggage
                {
                    CabinBag = 1,
                    CheckedBag = 2
                }
            };

            That.Given(_ => BaseUiSteps.IClickOnPrivacyPopupWindowAction(PrivacyPopupWindowActions.Accept))
                .And(_ => BaseUiSteps.INavigateToPage(PageUriFields.Flights))
                .When(_ => FlightSearchUiSteps.IFillAdditionalTravelInfoSection(flightSearchDataModel))
                .When(_ => FlightSearchUiSteps.IClickOnOriginPanel())
                .And(_ => FlightSearchUiSteps.ISelectOriginFromDropdown(flightSearchDataModel))
                .And(_ => FlightSearchUiSteps.IClickOnDestinationPanel())
                .And(_ => FlightSearchUiSteps.ISelectDestinationFromDropdown(flightSearchDataModel))
                .And(_ => FlightSearchUiSteps.IClickOnDepartureDateInput())
                .And(_ => FlightSearchUiSteps.IEnterDepartureDate(flightSearchDataModel))
                .And(_ => FlightSearchUiSteps.IClickOnReturnDateInput())
                .And(_ => FlightSearchUiSteps.IEnterReturnDate(flightSearchDataModel))
                .And(_ => FlightSearchUiSteps.IClickSearchFlightsButton())
                .Then(_ => FlightSearchResultUiSteps.IShouldSeeTheFlightResultSection())
                .BDDfy("Verify user is able to search for a flight with additional valid info");
        }
    }
}