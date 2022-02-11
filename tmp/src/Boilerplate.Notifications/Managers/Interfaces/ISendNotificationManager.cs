using System.Collections.Generic;
using System.Threading.Tasks;
using Boilerplate.Notifications.Models;

namespace Boilerplate.Notifications.Managers.Interfaces
{
    public interface ISendNotificationManager
    {
        Task<bool> SendPushNotification(Notification message);
        Task<bool> SendPushNotifications(IEnumerable<Notification> messages);
    }
}
