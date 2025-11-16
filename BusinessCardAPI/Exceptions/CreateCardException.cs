using System;

namespace BusinessCardAPI.Exceptions
{
    public class CreateCardException : Exception
    {
        public CreateCardException(string message) : base(message)
        {
        }

        public CreateCardException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}