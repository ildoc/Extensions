using Infrastructure.Authorization;

namespace Infrastructure.MassTransit.Commands
{
    public interface IGetUserInfoCommand : ICommandMessage
    {
        string UserId { get; set; }
    }

    public interface IGetUserInfoResult : ICommandResultMessage
    {
        UserInfo User { get; set; }
    }
}
