using System;
using System.Runtime.Serialization;

namespace BlingIntegrationTagplus.Exceptions
{
    [Serializable]
    public class ClienteException : Exception
    {
        public ClienteException(string message) : base(message) { }

        protected ClienteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
