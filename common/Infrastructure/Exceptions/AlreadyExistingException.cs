using System;

namespace Boilerplate.Common.Exceptions
{
    public class AlreadyExistingException : Exception
    {
        public AlreadyExistingException(string name, object key)
            : base($"Entity '{name}' ({key}) already exists.")
        {
        }
    }
}
