using System;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class FindByAttribute : Attribute
    {
        public FindByAttribute(string locator)
        {
            Locator = locator;
        }

        public string Locator { get; set; }
    }
}