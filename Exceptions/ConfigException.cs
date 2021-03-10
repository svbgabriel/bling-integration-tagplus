using System;

namespace BlingIntegrationTagplus.Exceptions
{
    [Serializable]
    public class ConfigException : Exception
    {
        public ConfigException(string message) : base(message) { }
    }
}
