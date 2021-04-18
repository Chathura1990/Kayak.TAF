using System;

namespace Core.Exceptions
{
    public class ElementWaitTimeoutException : Exception
    {
        public override string Message { get; }

        public ElementWaitTimeoutException(string message)
        {
            Message = message;
        }
    }
}