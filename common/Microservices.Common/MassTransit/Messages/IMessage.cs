namespace Microservices.Common.MassTransit.Messages
{
    public interface IMessage
    {
        string Reciever { get; }
    }
}
