using System;
using System.Runtime.Serialization;

namespace BlingIntegrationTagplus.Exceptions
{
    [Serializable]
    public class SituacaoException : Exception
    {
        public SituacaoException(string message) : base(message) { }

        protected SituacaoException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
