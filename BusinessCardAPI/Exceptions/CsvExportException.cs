using System;

namespace BusinessCardAPI.Exceptions
{
    public class CsvExportException : Exception
    {
        public CsvExportException(string message) : base(message)
        {
        }

        public CsvExportException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}