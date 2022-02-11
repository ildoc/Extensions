using System.Collections.Generic;
using System.Linq;
using Boilerplate.Common.Authorization;

namespace Boilerplate.Common.SignalR
{
    public static class Extensions
    {
        public static string GetUserGroup(this UserInfo u) => u.UserId.GetUserGroupByUserId();
        public static string GetUserGroupByUserId(this string userId) => $"user-{userId}";
        public static IEnumerable<string> GetRoleGroups(this UserInfo u) => u.Roles.Select(r => $"role-{r}");
    }
}
