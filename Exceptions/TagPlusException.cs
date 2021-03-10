using System;
using System.Runtime.Serialization;

namespace BlingIntegrationTagplus.Exceptions
{
    [Serializable]
    public class TagPlusException : Exception
    {
        public TagPlusException(string message) : base(message) { }

        protected TagPlusException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
