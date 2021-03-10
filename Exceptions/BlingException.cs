using System;
using System.Runtime.Serialization;

namespace BlingIntegrationTagplus.Exceptions
{
    [Serializable]
    public class BlingException : Exception
    {
        public BlingException(string message) : base(message) { }

        protected BlingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
