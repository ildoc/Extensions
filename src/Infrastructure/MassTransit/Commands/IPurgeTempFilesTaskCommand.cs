using Infrastructure.MassTransit.Messages;

namespace Infrastructure.MassTransit.Commands
{
    public interface IPurgeTempFilesTaskCommand : ICommandMessage, IScheduledTask { }
    public interface IPurgeTempFilesTaskResult : ICommandResultMessage, IScheduledTask { }
}
