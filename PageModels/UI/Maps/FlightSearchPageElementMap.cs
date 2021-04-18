using Core.Attributes;
using Core.Configurations;
using Core.UI;
using OpenQA.Selenium;
using PageModels.UI.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utils;
using OpenQA.Selenium.DevTools.V88.IndexedDB;
using PageModels.Models;

namespace PageModels.UI.Maps
{
    public class FlightSearchPageElementMap : BasePageElementMap
    {
        [FindBy(".//section[@class='form-section noBg']//div[contains(@id,'switch-display')][@role='button']")]
        public Element TripTypes;

        [FindBy(".//section[@class='form-section noBg']//button[contains(@id,'travelersAboveForm-dialog-trigger')]")]
        public Element TraverlersCount;
        
        [FindBy(".//section[@class='form-section noBg']//div[contains(@id,'cabinType')][@role='button']")]
        public Element CabinClass;
        
        [FindBy(".//section[@class='form-section noBg']//button[contains(@id,'baggage-dialog-trigger')]")]
        public Element BaggageCount;

        [FindBy(".//form[@name='rtow-searchform']//div[@aria-label='Flight origin input']")]
        public Element FlightOriginPanel;

        [FindBy(".//input[contains(@id,'origin-airport')][@aria-label='Flight origin input'][@aria-expanded='true']/..")]
        public DropdownWithSearch FlightOriginInput;

        [FindBy(".//form[@name='rtow-searchform']//div[@aria-label='Flight destination input']/parent::div[contains(@id,'destination-input-wrapper')]")]
        public Element FlightDestinationPanel;

        [FindBy(".//input[contains(@id,'destination-airport')][@aria-label='Flight destination input'][@aria-expanded='true']")]
        public Element FlightDestinationInput;

        [FindBy(".//form[@name='rtow-searchform']//div[@aria-label='Departure date input']")]
        public Element DepartureDatePanel;

        [FindBy(".//div[contains(@id,'depart-input')][@aria-label='Departure date input']")]
        public Element DepartureDateInput;

        [FindBy(".//div[contains(@id,'dates-col')]//div[@aria-label='Return date input']")]
        public Element ReturnDatePanel;

        [FindBy(".//div[contains(@id,'return-input')][@aria-label='Return date input']")]
        public Element ReturnDateInput;

        [FindBy(".//form[@name='rtow-searchform']//button[contains(@id,'submit')][@aria-label='Search flights']")]
        public Button SearchFlightsButton;

        [FindBy(".//div[@id='stl-jam-cal']//div[contains(@id,'stl-jam-cal') and (@aria-hidden='false')][1]")]
        public Element MonthsFromContainer;

        [FindBy(".//div[@id='stl-jam-cal']//div[contains(@id,'stl-jam-cal') and (@aria-hidden='false')][2]")]
        public Element MonthsToContainer;

        [FindBy(".//button[contains(@class,'react-country-picker-trigger')]")]
        public Button CountryPickerButton;

        private const string TripTypeOption = ".//div[contains(@id,'switch-content')]/div//li[contains(text(),'{0}')]";
        private const string TravelerIncrementer = ".//div[contains(@id,'travelersAboveForm-{0}')]//button[@title='Increment']";
        private const string TravelerElementValue = ".//div[contains(@id,'travelersAboveForm-{0}')]//input";
        private const string CabinClassOption = ".//div[contains(@id,'cabinType-widget-content')]/div//li[contains(text(),'{0}')]";
        private const string CabinClassIncrementer = "(.//div[contains(@id,'baggage-{0}')]//button[@title='Increment'])[2]";
        private const string DestinationSelectOption = ".//div[contains(@id,'destination-airport-smarty-content')]//li[@data-apicode='{0}']";
        private const string WeeksContainer = "/div[@class='month ']/div[@class='weeks ']";
        private const string ErrorMessage = ".//div[@class='errorContent']//li[contains(text(),\"Please enter a '{0}' airport.\")]";
        private const string SelectLanguage = ".//div[@class='react-country-picker-content']//span[contains(text(),'{0}')]";

        internal void FillAdditionalTravelInfo(FlightSearchDataModel additionalInfo)
        {
            SelectTripType(additionalInfo);
            SelectTravelers(additionalInfo);
            SelectCabinClass(additionalInfo);
            SelectBaggage(additionalInfo);
        }

        private void SelectTripType(FlightSearchDataModel additionalInfo)
        {
            TripTypes.Click();
            FindElement(string.Format(TripTypeOption, additionalInfo.TripType)).Click();
        }

        private void SelectTravelers(FlightSearchDataModel travelerInfo)
        {
            TraverlersCount.Click();
            IncrementAdults(travelerInfo);
            IncrementStudents(travelerInfo);
            IncrementYouths(travelerInfo);
            IncrementChildren(travelerInfo);
            IncrementToddlers(travelerInfo);
            IncrementInfants(travelerInfo);
            TraverlersCount.SendKeys(Keys.Enter);
        }

        private void SelectCabinClass(FlightSearchDataModel additionalInfo)
        {
            CabinClass.Click();
            FindElement(string.Format(CabinClassOption, additionalInfo.CabinClass)).Click();
        }

        private void SelectBaggage(FlightSearchDataModel additionalInfo)
        {
            BaggageCount.Click();
            IncrementCabinBag(additionalInfo);
            IncrementCheckedBag(additionalInfo);
            BaggageCount.SendKeys(Keys.Enter);
        }

        private void IncrementCabinBag(FlightSearchDataModel additionalInfo)
        {
            if (!additionalInfo.Baggage.CabinBag.Equals(null))
            {
                for (int i = 0; i < additionalInfo.Baggage.CabinBag; i++)
                {
                    Element incrementCabinBags = FindElement(string.Format(CabinClassIncrementer, "carry-on"));
                    incrementCabinBags.WaitForToBeClickable();
                    incrementCabinBags.Click();
                }
            }
        }
        
        private void IncrementCheckedBag(FlightSearchDataModel additionalInfo)
        {
            if (!additionalInfo.Baggage.CheckedBag.Equals(null))
            {
                for (int i = 0; i < additionalInfo.Baggage.CheckedBag; i++)
                {
                    Element incrementCheckedBags = FindElement(string.Format(CabinClassIncrementer, "checked-bag"));
                    incrementCheckedBags.WaitForToBeClickable();
                    incrementCheckedBags.Click();
                }
            }
        }

        private void IncrementAdults(FlightSearchDataModel travelers)
        {
            int count = travelers.Travelers.Adults;
            int actualValue = Convert.ToInt32(FindElement(string.Format(TravelerElementValue, "adults")).GetAttribute("Value"));

            if (!count.Equals(null) && !actualValue.Equals(count))
            {
                if (actualValue.Equals(1))
                {
                    count = count - 1;
                    for (int i = 0; i < count; i++)
                    {
                        Element incrementAdult = FindElement(string.Format(TravelerIncrementer, "adults"));
                        incrementAdult.WaitForToBeClickable();
                        incrementAdult.Click();
                    }
                }else
                {
                    for (int i = 0; i < count; i++)
                    {
                        Element incrementAdult = FindElement(string.Format(TravelerIncrementer, "adults"));
                        incrementAdult.WaitForToBeClickable();
                        incrementAdult.Click();
                    }
                }
            }
        }

        private void IncrementStudents(FlightSearchDataModel travelers)
        {
            if (!travelers.Travelers.Students.Equals(null))
            {
                for (int i = 0; i < travelers.Travelers.Students; i++)
                {
                    Element incrementStudents = FindElement(string.Format(TravelerIncrementer, "students"));
                    incrementStudents.WaitForToBeClickable();
                    incrementStudents.Click();
                }
            }
        }

        private void IncrementYouths(FlightSearchDataModel travelers)
        {
            if (!travelers.Travelers.Youths.Equals(null))
            {
                for (int i = 0; i < travelers.Travelers.Youths; i++)
                {
                    Element incrementYouth = FindElement(string.Format(TravelerIncrementer, "youth"));
                    incrementYouth.WaitForToBeClickable();
                    incrementYouth.Click();
                }
            }
        }

        private void IncrementChildren(FlightSearchDataModel travelers)
        {
            if (!travelers.Travelers.Children.Equals(null))
            {
                for (int i = 0; i < travelers.Travelers.Children; i++)
                {
                    Element incrementChild = FindElement(string.Format(TravelerIncrementer, "child"));
                    incrementChild.WaitForToBeClickable();
                    incrementChild.Click();
                }
            }
        }

        private void IncrementToddlers(FlightSearchDataModel travelers)
        {
            if (!travelers.Travelers.ToddlerInOwnSeat.Equals(null))
            {
                for (int i = 0; i < travelers.Travelers.ToddlerInOwnSeat; i++)
                {
                    Element incrementSeatInfant = FindElement(string.Format(TravelerIncrementer, "seatInfant"));
                    incrementSeatInfant.WaitForToBeClickable();
                    incrementSeatInfant.Click();
                }
            }
        }

        private void IncrementInfants(FlightSearchDataModel travelers)
        {
            if (!travelers.Travelers.InfantOnLap.Equals(null))
            {
                for (int i = 0; i < travelers.Travelers.InfantOnLap; i++)
                {
                    Element incrementLapInfant = FindElement(string.Format(TravelerIncrementer, "lapInfant"));
                    incrementLapInfant.WaitForToBeClickable();
                    incrementLapInfant.Click();
                }
            }
        }

        internal void ClickCountryPicker()
        {
            CountryPickerButton.Click();
        }

        internal string GetCountryPickerButtonAttribute()
        {
            CountryPickerButton.WaitForToBeDisplayed();
            return CountryPickerButton.GetAttribute("aria-label");
        }

        internal void SelectACountryLanguage(string country)
        {
            Element language = FindElement(string.Format(SelectLanguage, country));
            language.Click();
        }

        internal void SelectDestinationOption(string optionValue)
        {
            if (!string.IsNullOrEmpty(optionValue))
            {
                FlightDestinationInput.SendKeys(Keys.Backspace);
                FlightDestinationInput.SendKeys(Keys.Backspace);
                FlightDestinationInput.Clear();
                FlightDestinationInput.SendKeys(optionValue);
                Element option = FindElement(String.Format(DestinationSelectOption, optionValue));
                option.WaitForToBeDisplayed(TimeSpan.FromSeconds(ConfigManager.Wait.DefaultTimeout));
                FlightDestinationInput.SendKeys(Keys.Enter);
            }
        }

        internal void EnterDepartureDate(string dateFrom)
        {
            DepartureDateInput.WaitForToBeDisplayed(TimeSpan.FromSeconds(ConfigManager.Wait.DefaultTimeout));
            DepartureDateInput.Clear();
            DepartureDateInput.SendKeys(dateFrom);
            DepartureDateInput.SendKeys(Keys.Enter);
        }

        internal void EnterReturnDate(string dateTo)
        {
            ReturnDateInput.WaitForToBeDisplayed(TimeSpan.FromSeconds(ConfigManager.Wait.DefaultTimeout));
            ReturnDateInput.Clear();
            ReturnDateInput.SendKeys(dateTo);
            ReturnDateInput.SendKeys(Keys.Enter);
        }

        internal Element CheckErrorMessage(string error)
        {
            return FindElement(string.Format(ErrorMessage, error));
        }

        internal void SelectFromDate(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                List<Element> columns = MonthsFromContainer.FindElements(WeeksContainer)
                    .WaitForAny(Condition.Displayed)
                    .ToList();

                foreach (Element cell in columns)
                {
                    if (cell.Text.Equals(date))
                    {
                        cell.FindElement($".//div[contains(text(),'{date}')]");
                        cell.WaitForToBeClickable(TimeSpan.FromSeconds(ConfigManager.Wait.DefaultTimeout));
                        cell.Click();
                        break;
                    }
                }
            }
        }

        internal void SelectToDate(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                List<Element> columns = MonthsToContainer.FindElements(WeeksContainer)
                    .WaitForAny(Condition.Displayed)
                    .ToList();

                foreach (Element cell in columns)
                {
                    if (cell.Text.Equals(date))
                    {
                        cell.FindElement($".//div[contains(text(),'{date}')]");
                        cell.WaitForToBeClickable(TimeSpan.FromSeconds(ConfigManager.Wait.DefaultTimeout));
                        cell.Click();
                        break;
                    }
                }
            }
        }
    }
}