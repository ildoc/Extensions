using Microservices.Common.MassTransit.Messages;

namespace Microservices.Common.MassTransit.Commands
{
    public interface ICommandMessage : IMessage { }
    public interface ICommandResultMessage : IMessage
    {
        int Result { get; set; }
    }
}
