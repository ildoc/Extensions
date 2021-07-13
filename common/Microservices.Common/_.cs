using System;
using Microservices.Common.Exceptions;

namespace Microservices.Common
{
    public static class _
    {
        public static void ControlledTry(Action a, string message)
        {
            try
            {
                a();
            }
            catch
            {
                throw new BadRequestException(message);
            }
        }

        public static void ControlledTry(Action a, Exception e)
        {
            try
            {
                a();
            }
            catch
            {
                throw e;
            }
        }
    }
}
