using System;
using System.Runtime.Serialization;

namespace Task1
{
    [Serializable]
    internal class TodoAccessDeneidException : Exception
    {
        public TodoAccessDeneidException()
        {
        }

        public TodoAccessDeneidException(string message) : base(message)
        {
        }

        public TodoAccessDeneidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TodoAccessDeneidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}