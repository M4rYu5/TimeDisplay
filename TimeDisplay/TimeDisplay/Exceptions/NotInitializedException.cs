using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TimeDisplay.Exceptions
{
    public class NotInitializedException : Exception
    {
        public NotInitializedException()
        {
        }

        public NotInitializedException(string message) : base(message)
        {
        }

        public NotInitializedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotInitializedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
