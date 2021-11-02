using System;

namespace UniversityApp.Core.Exceptions
{
    public class DuplicatedEntityException : Exception
    {
        public DuplicatedEntityException() : base() { }

        public DuplicatedEntityException(string message) : base(message) { }

        public DuplicatedEntityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
