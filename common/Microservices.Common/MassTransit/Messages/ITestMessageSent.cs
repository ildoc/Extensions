namespace Microservices.Common.MassTransit.Messages
{
    public interface ITestMessageSent : IMessage
    {
        string Title { get; }
        int Level { get; }
    }
}
