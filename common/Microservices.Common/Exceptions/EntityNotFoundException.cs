using System;

namespace Microservices.Common.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(object key)
            : base($"Entity '{key.GetType().Name}' ({key}) was not found.")
        {
        }

        public EntityNotFoundException(string name, object key)
            : base($"Entity '{name}' ({key}) was not found.")
        {
        }

        public EntityNotFoundException(string msg) : base(msg) { }
    }
}
