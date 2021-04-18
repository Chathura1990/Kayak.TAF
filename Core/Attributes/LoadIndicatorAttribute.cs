using System;

namespace Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class LoadIndicatorAttribute : Attribute
    {
        public LoadIndicatorAttribute(bool waitVisible = true)
        {
            WaitVisible = waitVisible;
        }

        public bool WaitVisible { get; set; }
    }
}