using System;

namespace Boilerplate.Common.Exceptions
{
    public class RequestEntityTooLargeException : Exception
    {
        public RequestEntityTooLargeException(string message) : base(message)
        {
        }
    }
}
