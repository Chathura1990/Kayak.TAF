namespace PageModels.Models
{
    public class TravelersAboveForm
    {
        public int Adults { get; set; }
        /// <summary>
        ///   Over 18.
        /// </summary>
        public int Students { get; set; }
        /// <summary>
        ///   Between 12 to 15.
        /// </summary>
        public int Youths { get; set; }
        /// <summary>
        ///   Between 2 to 11.
        /// </summary>
        public int Children { get; set; }
        /// <summary>
        ///   Under 2.
        /// </summary>
        public int ToddlerInOwnSeat { get; set; }
        /// <summary>
        ///   Under 2.
        /// </summary>
        public int InfantOnLap { get; set; }
    }
}