namespace Boilerplate.Common.MassTransit.Messages
{
    public interface IMessage
    {
        string Reciever { get; }
    }
}
