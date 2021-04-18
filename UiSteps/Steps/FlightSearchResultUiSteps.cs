using Core;
using PageModels.UI;
using PageModels.UI.Pages;

namespace UiSteps.Steps
{
    public class FlightSearchResultUiSteps : BaseUiSteps
    {
        public FlightSearchResultUiSteps(StepsContext stepsContext) : base(stepsContext)
        {
        }
        public void IShouldSeeTheFlightResultSection()
        {
            PageFactory.GetPage<FlightSearchResultPage>().Validate().FlightResultSection();
        }
    }
}