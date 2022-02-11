namespace Infrastructure.MassTransit.Messages
{
    public interface IMessage
    {
        string Reciever { get; }
    }
}
