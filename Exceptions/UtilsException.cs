using System;
using System.Runtime.Serialization;

namespace BlingIntegrationTagplus.Exceptions
{
    [Serializable]
    public class UtilsException : Exception
    {
        public UtilsException(string message) : base(message) { }

        protected UtilsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
