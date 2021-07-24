using System;
using System.Runtime.Serialization;

namespace TimeDisplay.Exceptions
{
    public class ThemeNotFoundException : Exception
    {
        public ThemeNotFoundException()
        {
        }

        public ThemeNotFoundException(string message) : base(message)
        {
        }

        public ThemeNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ThemeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
