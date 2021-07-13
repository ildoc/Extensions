using System;

namespace Microservices.Common.Exceptions
{
    public class ForbiddenActionException : Exception
    {
        public ForbiddenActionException(string message) : base(message)
        {
        }
    }
}
