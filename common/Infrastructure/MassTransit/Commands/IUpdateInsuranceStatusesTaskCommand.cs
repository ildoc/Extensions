using Boilerplate.Common.MassTransit.Messages;

namespace Boilerplate.Common.MassTransit.Commands
{
    public interface IUpdateInsuranceStatusesTaskCommand : ICommandMessage, IScheduledTask { }
    public interface IUpdateInsuranceStatusesTaskResult : ICommandResultMessage, IScheduledTask { }
}
