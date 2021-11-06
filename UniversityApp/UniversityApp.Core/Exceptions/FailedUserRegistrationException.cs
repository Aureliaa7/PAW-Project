using System;

namespace UniversityApp.Core.Exceptions
{
    public class FailedUserRegistrationException : Exception
    {
        public FailedUserRegistrationException() : base() { }

        public FailedUserRegistrationException(string message) : base(message) { }

        public FailedUserRegistrationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
