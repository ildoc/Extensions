using Microservices.Common.MassTransit.Messages;

namespace Microservices.Common.MassTransit.Commands
{
    public interface IPurgeTempFilesTaskCommand : ICommandMessage, IScheduledTask { }
    public interface IPurgeTempFilesTaskResult : ICommandResultMessage, IScheduledTask { }
}
