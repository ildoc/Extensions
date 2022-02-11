using Infrastructure.MassTransit.Messages;

namespace Infrastructure.MassTransit.Commands
{
    public interface ICommandMessage : IMessage { }
    public interface ICommandResultMessage : IMessage
    {
        int Result { get; set; }
    }
}
