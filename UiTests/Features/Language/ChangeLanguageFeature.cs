using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Core.Extensions;
using NUnit.Framework;
using PageModels.App.StaticData;
using PageModels.App.StaticData.AirportCodes;
using PageModels.Models;
using TestStack.BDDfy;

namespace UiTests.Features.Language
{
    public class ChangeLanguageFeature : UiTestHelper
    {
        public ChangeLanguageFeature()
        {
            Logger.Debug($"[Feature] {GetType()}");
        }

        [Test, TestCaseSource(nameof(TestDataCountries))]
        [Category(TestCategories.Smoke)]
        [Category(TestCategories.High)]
        public void VerifyUserIsAbleToChangeTheSiteLanguage(string country)
        {
            That.Given(_ => BaseUiSteps.IClickOnPrivacyPopupWindowAction(PrivacyPopupWindowActions.NoThanks))
                .And(_ => BaseUiSteps.INavigateToPage(PageUriFields.Flights))
                .When(_ => FlightSearchUiSteps.IClickOnCountryPickerButton())
                .And(_ => FlightSearchUiSteps.ISelectACountryLanguage(country))
                .Then(_ => FlightSearchUiSteps.IShouldSeeLanguageIsSelected(country))
                .BDDfy("Verify user is able to change the site language");
        }

        private static IEnumerable<TestCaseData> TestDataCountries()
        {
            List<String> countries = new List<string>();
            countries.Add(AvailableCountryLanguages.Argentina);
            countries.Add(AvailableCountryLanguages.Denmark);
            countries.Add(AvailableCountryLanguages.Greece);
            countries.Add(AvailableCountryLanguages.Italy);
            countries.Add(AvailableCountryLanguages.China);
            foreach (var country in countries)
            {
                yield return new TestCaseData(country);
            }
        }
    }
}
