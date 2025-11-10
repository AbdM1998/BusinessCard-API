using System;

namespace BusinessCardAPI.Exceptions
{
    public class XmlParsingException : Exception
    {
        public XmlParsingException(string message) : base(message)
        {
        }

        public XmlParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}