using System;
using System.Runtime.Serialization;

namespace UniversityApp.Core.Exceptions
{
    [Serializable]
    public class IncorrectCredentialsException : Exception
    {
        protected IncorrectCredentialsException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public IncorrectCredentialsException() : base() { }

        public IncorrectCredentialsException(string message) : base(message) { }

        public IncorrectCredentialsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
