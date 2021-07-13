namespace Microservices.Common.MassTransit.Messages
{
    public interface IUserPrimaryDataChanged : IMessage
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
