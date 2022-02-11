using System;

namespace Boilerplate.Common.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException(string message)
            : base(message)
        {
        }
    }
}
