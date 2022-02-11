using System.Collections.Generic;
using System.Threading.Tasks;
using Boilerplate.Common.Authorization;
using Boilerplate.Common.Authorization;
using Boilerplate.Notifications.Models;

namespace Boilerplate.Notifications.Managers.Interfaces
{
    public interface INotificationManager
    {
        Task<Notification> AddNotification(Notification notification);
        IEnumerable<Notification> GetNotifications(UserInfo user);
        IEnumerable<Notification> GetUnreadNotifications(UserInfo user);
        Task<IEnumerable<Notification>> MarkAllAsRead(UserInfo user);
        Task<Notification> MarkAsRead(string notificationGuid, UserInfo user);
        Task<IEnumerable<Notification>> AddNotifications(IEnumerable<Notification> notifications);
    }
}
