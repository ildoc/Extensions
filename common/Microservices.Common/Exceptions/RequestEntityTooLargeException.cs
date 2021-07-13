using System;

namespace Microservices.Common.Exceptions
{
    public class RequestEntityTooLargeException : Exception
    {
        public RequestEntityTooLargeException(string message) : base(message)
        {
        }
    }
}
