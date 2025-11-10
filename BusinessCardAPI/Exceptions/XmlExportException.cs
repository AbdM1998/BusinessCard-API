using System;

namespace BusinessCardAPI.Exceptions
{
    public class XmlExportException : Exception
    {
        public XmlExportException(string message) : base(message)
        {
        }

        public XmlExportException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}