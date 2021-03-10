using System;
using System.Runtime.Serialization;

namespace BlingIntegrationTagplus.Exceptions
{
    [Serializable]
    public class ConfigException : Exception
    {
        public ConfigException(string message) : base(message) { }

        protected ConfigException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
