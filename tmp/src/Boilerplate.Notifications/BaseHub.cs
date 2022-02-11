using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boilerplate.Common.Authorization;
using Boilerplate.Common.Keycloak;
using Boilerplate.Common.MassTransit;
using Boilerplate.Common.MassTransit.Commands;
using Boilerplate.Common.SignalR;
using Boilerplate.Notifications.Managers;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Boilerplate.Notifications
{
    [Authorize]
    public class BaseHub : Hub
    {
        private readonly IKeycloakClient _keycloakClient;
        private readonly ILogger<BaseHub> _logger;
        private readonly InMemoryStore _inMemoryStore;
        private readonly MassTransitOptions _masstransitOptions;
        private readonly RabbitMqQueues _rabbitMqQueues;
        private readonly IBus _bus;

        public BaseHub(IKeycloakClient keycloakClient, ILogger<BaseHub> logger, InMemoryStore inMemoryStore, IOptions<MassTransitOptions> masstransitOptions,
            IOptions<RabbitMqQueues> rabbitMqQueues, IBus bus)
        {
            _keycloakClient = keycloakClient;
            _logger = logger;
            _inMemoryStore = inMemoryStore;
            _masstransitOptions = masstransitOptions.Value;
            _rabbitMqQueues = rabbitMqQueues.Value;
            _bus = bus;
        }

        public override async Task<Task> OnConnectedAsync()
        {
            var user = Context.User.Claims.GetUserInfo();

            var client = _bus.CreateRequestClient<IGetUserInfoCommand>(new Uri($"{_masstransitOptions.ServerUri}/{_rabbitMqQueues.Api}"));
            var response = await client.GetResponse<IGetUserInfoResult>(new { user.UserId });

            return await AddGroupsAsync(response.Message.User)
                .ContinueWith(_ => base.OnConnectedAsync());
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var cid = Context.ConnectionId;
            _inMemoryStore.RemoveClient(cid);

            return base.OnDisconnectedAsync(exception);
        }

        private async Task AddGroupsAsync(UserInfo user)
        {
            var cid = Context.ConnectionId;

            _inMemoryStore.AddClient(cid, user);

            //add to user group (single group foreach user)
            await Groups.AddToGroupAsync(cid, user.GetUserGroup());
            //one group foreach associated role (usually 1 for user)
            foreach (var g in user.GetRoleGroups())
                await Groups.AddToGroupAsync(cid, g);

            var groups = new List<string> { user.GetUserGroup() }.Concat(user.GetRoleGroups());

            _logger.LogInformation($"User {user.TaxCode} has been registered in groups \"{string.Join("\", \"", groups)}\"");
        }
    }
}
