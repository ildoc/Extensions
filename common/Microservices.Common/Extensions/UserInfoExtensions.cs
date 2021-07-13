using System;
using System.Linq;
using Microservices.Common.Authorization;
using Microservices.Common.Types.Const;

namespace Microservices.Common.Extensions
{
    public static class UserInfoExtensions
    {
        public static bool IsAdmin(this UserInfo user) =>
            user.Roles?.Contains(RoleType.ADMIN) == true;
    }
}
