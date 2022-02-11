using System;

namespace Boilerplate.Common.Exceptions
{
    public class ForbiddenActionException : Exception
    {
        public ForbiddenActionException(string message) : base(message)
        {
        }
    }
}
