using Core.Selenium.Browser;
using System;

namespace Core.Configurations
{
    public class BrowserConfiguration : IConfiguration
    {
        private const string SectionName = "Browser";
        public string JsonSectionName => SectionName;
        public string StartUrl { get; set; }
        public bool Headless { get; set; }
        public int WindowWidth { get; set; }
        public int WindowHeight { get; set; }
        public BrowserType Type { get; } = (BrowserType)Enum.Parse(typeof(BrowserType), ConfigManager.GetValue($"{SectionName}:Type"));
    }
}