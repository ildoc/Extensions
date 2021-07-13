using System;

namespace Microservices.Common.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException(string message)
            : base(message)
        {
        }
    }
}
