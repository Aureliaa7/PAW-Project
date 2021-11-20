using System;
using System.Runtime.Serialization;

namespace UniversityApp.Core.Exceptions
{
    [Serializable]
    public class DuplicatedEntityException : Exception
    {
        protected DuplicatedEntityException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public DuplicatedEntityException() : base() { }

        public DuplicatedEntityException(string message) : base(message) { }

        public DuplicatedEntityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
