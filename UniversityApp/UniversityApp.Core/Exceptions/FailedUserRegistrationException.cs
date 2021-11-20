using System;
using System.Runtime.Serialization;

namespace UniversityApp.Core.Exceptions
{
    [Serializable]
    public class FailedUserRegistrationException : Exception
    {
        protected FailedUserRegistrationException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public FailedUserRegistrationException() : base() { }

        public FailedUserRegistrationException(string message) : base(message) { }

        public FailedUserRegistrationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
