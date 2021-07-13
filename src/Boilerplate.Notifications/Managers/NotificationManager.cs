using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extensions;
using Boilerplate.Common.Authorization;
using Boilerplate.Common.Settings;
using Boilerplate.Common.SignalR;
using Boilerplate.Common.Types.Interfaces;
using Boilerplate.Notifications.Enums;
using Boilerplate.Notifications.Managers.Interfaces;
using Boilerplate.Notifications.Models;
using Boilerplate.Notifications.Settings;

namespace Boilerplate.Notifications.Managers
{
    public class NotificationManager : INotificationManager
    {
        private readonly IRepository<Notification> _notificationRepository;
        private readonly IApplicationSettingsManager _settingsManager;

        public NotificationManager(IRepository<Notification> notificationRepository, IApplicationSettingsManager settingsManager)
        {
            _notificationRepository = notificationRepository;
            _settingsManager = settingsManager;
        }

        public IEnumerable<Notification> GetNotifications(UserInfo user)
        {
            var expireAfter = _settingsManager.GetSetting(NotificationApplicationSettings.NOTIFICATIONS_EXPIRE_AFTER)?.ToInt32();
            return _notificationRepository.GetAll().Where(x => x.Reciever == user.UserId.GetUserGroupByUserId()).WhereIf(expireAfter != default, x => x.CreationDate.AddDays(expireAfter.Value) >= DateTime.Today);
        }

        public IEnumerable<Notification> GetUnreadNotifications(UserInfo user) =>
            GetNotifications(user).Where(x => x.Status == StatusEnum.New);

        public async Task<Notification> AddNotification(Notification notification)
        {
            notification.Guid = Guid.NewGuid().ToString();
            await _notificationRepository.AddAsync(notification);

            return notification;
        }

        public async Task<Notification> MarkAsRead(string notificationGuid, UserInfo user)
        {
            var notification = GetUnreadNotifications(user)
                .FirstOrDefault(x => x.Guid == notificationGuid);

            if (notification == default)
                return default;

            notification.Status = StatusEnum.Read;

            await _notificationRepository.UpdateAsync(notification);

            return notification;
        }

        public async Task<IEnumerable<Notification>> MarkAllAsRead(UserInfo user)
        {
            var notifications = GetUnreadNotifications(user)
                .Select(x => { x.Status = StatusEnum.Read; return x; });

            await _notificationRepository.UpdateRangeAsync(notifications);

            return notifications;
        }

        public async Task<IEnumerable<Notification>> AddNotifications(IEnumerable<Notification> notifications)
        {
            await _notificationRepository.AddRangeAsync(notifications.Select(x => { x.Guid = Guid.NewGuid().ToString(); return x; }));

            return notifications;
        }
    }
}
