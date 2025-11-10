using System;

namespace BusinessCardAPI.Exceptions
{
    public class CsvParsingException : Exception
    {
        public CsvParsingException(string message) : base(message)
        {
        }

        public CsvParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}