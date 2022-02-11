using Boilerplate.Common.MassTransit.Messages;

namespace Boilerplate.Common.MassTransit.Commands
{
    public interface IPurgeTempFilesTaskCommand : ICommandMessage, IScheduledTask { }
    public interface IPurgeTempFilesTaskResult : ICommandResultMessage, IScheduledTask { }
}
