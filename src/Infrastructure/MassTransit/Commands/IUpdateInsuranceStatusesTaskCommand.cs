using Infrastructure.MassTransit.Messages;

namespace Infrastructure.MassTransit.Commands
{
    public interface IUpdateInsuranceStatusesTaskCommand : ICommandMessage, IScheduledTask { }
    public interface IUpdateInsuranceStatusesTaskResult : ICommandResultMessage, IScheduledTask { }
}
