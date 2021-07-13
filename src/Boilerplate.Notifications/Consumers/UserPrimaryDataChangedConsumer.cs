using System.Threading.Tasks;
using MassTransit;
using Boilerplate.Common.MassTransit.Messages;
using Boilerplate.Common.Types.Enums;
using Microsoft.Extensions.DependencyInjection;
using Boilerplate.Notifications.Managers.Interfaces;
using Boilerplate.Notifications.Models;

namespace Boilerplate.Notifications.Consumers
{
    public class UserPrimaryDataChangedConsumer : IConsumer<IUserPrimaryDataChanged>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public UserPrimaryDataChangedConsumer(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task Consume(ConsumeContext<IUserPrimaryDataChanged> context)
        {
            using var scope = _scopeFactory.CreateScope();
            var sendNotificationManager = scope.ServiceProvider.GetService<ISendNotificationManager>();

            var notification = new Notification
            {
                Title = context.Message.Title,
                Description = context.Message.Description,
                Level = NotificationLevelEnum.Info,
                Reciever = context.Message.Reciever
            };

            await sendNotificationManager.SendPushNotification(notification);
        }
    }
}
