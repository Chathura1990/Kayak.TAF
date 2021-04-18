namespace PageModels.Models
{
    public class FlightSearchDataModel
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string TripType { get; set; }
        public TravelersAboveForm Travelers { get; set; }
        public string CabinClass { get; set; }
        public Baggage Baggage { get; set; }
    }
}