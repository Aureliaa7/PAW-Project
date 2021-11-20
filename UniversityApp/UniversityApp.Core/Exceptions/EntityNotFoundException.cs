using System;
using System.Runtime.Serialization;

namespace UniversityApp.Core.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public EntityNotFoundException() : base() { }

        public EntityNotFoundException(string message) : base(message) { }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
