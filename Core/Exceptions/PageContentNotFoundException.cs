using System;

namespace Core.Exceptions
{
    public class PageContentNotFoundException : Exception
    {
        public override string Message { get; }

        public PageContentNotFoundException(string message)
        {
            Message = message;
        }
    }
}