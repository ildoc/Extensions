using System;

namespace Infrastructure.Exceptions
{
    public class AlreadyExistingException : Exception
    {
        public AlreadyExistingException(string name, object key)
            : base($"Entity '{name}' ({key}) already exists.")
        {
        }
    }
}
