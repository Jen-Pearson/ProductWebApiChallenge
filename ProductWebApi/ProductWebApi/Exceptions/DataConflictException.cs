using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ProductWebApi.Exceptions
{
    public class DataConflictException : Exception
    {
        public DataConflictException()
        {
        }

        public DataConflictException(string message) : base(message)
        {
        }

        public DataConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DataConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
