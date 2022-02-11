using System.Collections.Generic;
using Boilerplate.Common.Authorization;

namespace Boilerplate.Notifications.Managers
{
    public class InMemoryStore
    {
        private readonly Dictionary<string, UserInfo> _connectedClients;

        public InMemoryStore()
        {
            _connectedClients = new Dictionary<string, UserInfo>();
        }

        internal bool RemoveClient(string cid) => _connectedClients.Remove(cid);

        internal UserInfo GetUser(string cid) =>
           _connectedClients.GetValueOrDefault(cid);

        internal void AddClient(string cid, UserInfo user) => _connectedClients.Add(cid, user);
    }
}
