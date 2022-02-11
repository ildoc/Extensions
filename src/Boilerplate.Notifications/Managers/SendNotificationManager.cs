using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boilerplate.Notifications.Managers.Interfaces;
using Boilerplate.Notifications.Models;
using Microsoft.AspNetCore.SignalR;

namespace Boilerplate.Notifications.Managers
{
    public class SendNotificationManager : ISendNotificationManager
    {
        private readonly INotificationManager _notificationManager;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public SendNotificationManager(INotificationManager notificationManager, IHubContext<NotificationHub> notificationHub)
        {
            _notificationManager = notificationManager;
            _notificationHub = notificationHub;
        }

        public Task<bool> SendPushNotification(Notification notification) =>
           SendPushNotifications(new List<Notification> { notification });

        public async Task<bool> SendPushNotifications(IEnumerable<Notification> notifications)
        {
            var messages = await _notificationManager.AddNotifications(notifications);
            try
            {
                await Task.WhenAll(messages.Select(message => _notificationHub.Clients.Groups(message.Reciever).SendAsync(
                        message.Method,
                        new
                        {
                            message.Guid,
                            message.Title,
                            message.Description,
                            message.Level,
                            message.Status,
                            message.TimeStamp,
                            message.DeadLine,
                            message.Payload
                        }
                    )
           ));
                return true;
            }
            catch { return false; }
        }
    }
}
