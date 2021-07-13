using System;

namespace Microservices.Common.Exceptions
{
    public class AlreadyExistingException : Exception
    {
        public AlreadyExistingException(string name, object key)
            : base($"Entity '{name}' ({key}) already exists.")
        {
        }
    }
}
