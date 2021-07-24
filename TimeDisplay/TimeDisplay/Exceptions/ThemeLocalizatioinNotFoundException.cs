using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TimeDisplay.Exceptions
{
    public class ThemeLocalizatioinNotFoundException : Exception
    {
        public ThemeLocalizatioinNotFoundException()
        {
        }

        public ThemeLocalizatioinNotFoundException(string message) : base(message)
        {
        }

        public ThemeLocalizatioinNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ThemeLocalizatioinNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
