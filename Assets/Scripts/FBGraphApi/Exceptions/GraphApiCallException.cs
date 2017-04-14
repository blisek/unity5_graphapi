using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Assets.Scripts.FBGraphApi.Exceptions
{
    public class GraphApiCallException : SystemException
    {
        public int ErrorId { get; private set; }

        public GraphApiCallException()
        {
        }

        public GraphApiCallException(int errorId, string message) : base(message)
        {
            ErrorId = errorId;
        }

        public GraphApiCallException(int errorId, string message, Exception innerException) : base(message, innerException)
        {
            ErrorId = errorId;
        }

        protected GraphApiCallException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
