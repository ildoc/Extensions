namespace Infrastructure.Exceptions
{
    public class ForbiddenActionException : Exception
    {
        public ForbiddenActionException(string message) : base(message)
        {
        }
    }
}
