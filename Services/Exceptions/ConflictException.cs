using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Services.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException() { }

        public ConflictException(string message) : base(message) { }

        public ConflictException(string message, Exception innerException) : base(message, innerException) { }

        protected ConflictException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public static ConflictException Create(string message)
        {
            return new ConflictException(message);
        }

        public static ConflictException Create(Exception innerException)
        {
            return new ConflictException("Conflict: " + innerException.Message, innerException);
        }
    }

}
