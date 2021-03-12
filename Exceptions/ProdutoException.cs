using System;
using System.Runtime.Serialization;

namespace BlingIntegrationTagplus.Exceptions
{
    [Serializable]
    public class ProdutoException : Exception
    {
        public ProdutoException(string message) : base(message) { }

        protected ProdutoException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
