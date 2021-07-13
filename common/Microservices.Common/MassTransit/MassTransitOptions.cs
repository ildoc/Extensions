namespace Microservices.Common.MassTransit
{
    public class MassTransitOptions
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ServerUri { get; set; }
        public ushort PrefetchCount { get; set; }
    }
}
