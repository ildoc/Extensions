using Boilerplate.Common.MassTransit.Messages;

namespace Boilerplate.Common.MassTransit.Commands
{
    public interface ICommandMessage : IMessage { }
    public interface ICommandResultMessage : IMessage
    {
        int Result { get; set; }
    }
}
