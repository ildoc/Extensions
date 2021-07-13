using Microservices.Common.MassTransit.Messages;

namespace Microservices.Common.MassTransit.Commands
{
    public interface IUpdateInsuranceStatusesTaskCommand : ICommandMessage, IScheduledTask { }
    public interface IUpdateInsuranceStatusesTaskResult : ICommandResultMessage, IScheduledTask { }
}
