using System;
using System.Linq;
using Infrastructure.Authorization;
using Infrastructure.Types.Const;

namespace Infrastructure.Extensions
{
    public static class UserInfoExtensions
    {
        public static bool IsAdmin(this UserInfo user) =>
            user.Roles?.Contains(RoleType.ADMIN) == true;
    }
}
