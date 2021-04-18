using System;

namespace Core.Exceptions
{
    public class BrowserAlertNotFoundException : Exception
    {
        public override string Message { get; }

        public BrowserAlertNotFoundException(string message)
        {
            Message = message;
        }
    }
}