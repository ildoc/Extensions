using System.Threading.Tasks;
using Boilerplate.Common.Keycloak;
using Boilerplate.Common.MassTransit;
using Boilerplate.Common.SignalR;
using Boilerplate.Notifications.Consts;
using Boilerplate.Notifications.Managers;
using Boilerplate.Notifications.Managers.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Boilerplate.Notifications
{
    public class NotificationHub : BaseHub
    {
        private readonly INotificationManager _notificationManager;
        private readonly InMemoryStore _inMemoryStore;

        public NotificationHub(INotificationManager notificationManager, IKeycloakClient keycloakClient, ILogger<NotificationHub> logger, InMemoryStore inMemoryStore,
            IOptions<MassTransitOptions> masstransitOptions, IOptions<RabbitMqQueues> rabbitMqQueues, IBus bus)
            : base(keycloakClient, logger, inMemoryStore, masstransitOptions, rabbitMqQueues, bus)
        {
            _notificationManager = notificationManager;
            _inMemoryStore = inMemoryStore;
        }

        public async Task MarkAsRead(string guid)
        {
            var user = _inMemoryStore.GetUser(Context.ConnectionId);
            if (user == default)
                return;

            await _notificationManager.MarkAsRead(guid, user);
        }

        public async Task MarkAllAsRead()
        {
            var user = _inMemoryStore.GetUser(Context.ConnectionId);
            if (user == default)
                return;

            await _notificationManager.MarkAllAsRead(user);
        }

        public async Task GetUnreadNotifications()
        {
            var user = _inMemoryStore.GetUser(Context.ConnectionId);
            if (user == default)
                return;

            var notifications = _notificationManager.GetUnreadNotifications(user);

            await Clients.Groups(new[] { user.GetUserGroup() }).SendAsync(
                         NotificationMethods.USERNOTIFICATIONS,
                         notifications
                     );
        }

        public async Task GetNotifications()
        {
            var user = _inMemoryStore.GetUser(Context.ConnectionId);
            if (user == default)
                return;

            var notifications = _notificationManager.GetNotifications(user);

            await Clients.Groups(new[] { user.GetUserGroup() }).SendAsync(
                         NotificationMethods.USERNOTIFICATIONS,
                         notifications
                     );
        }
    }
}
