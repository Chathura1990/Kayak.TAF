using System;

namespace Core.Exceptions
{
    public class InvalidateElementException : Exception
    {
        /// <inheritdoc />
        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidateElementException" /> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InvalidateElementException(string message)
        {
            Message = message;
        }
    }
}