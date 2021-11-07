using System;

namespace UniversityApp.Core.Exceptions
{
    public class IncorrectCredentialsException : Exception
    {
        public IncorrectCredentialsException() : base() { }

        public IncorrectCredentialsException(string message) : base(message) { }

        public IncorrectCredentialsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
