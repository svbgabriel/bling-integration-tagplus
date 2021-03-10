using System;
using System.Runtime.Serialization;

namespace BlingIntegrationTagplus.Exceptions
{
    [Serializable]
    public class TipoContatoException : Exception
    {
        public TipoContatoException(string message) : base(message) { }

        protected TipoContatoException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
