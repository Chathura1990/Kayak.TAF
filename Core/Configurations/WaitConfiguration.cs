namespace Core.Configurations
{
    public class WaitConfiguration : IConfiguration
    {
        public string JsonSectionName => "Wait";
        public double DefaultTimeout { get; set; }
        public double Timeout { get; set; }
        public double PollingInterval { get; set; }
    }
}