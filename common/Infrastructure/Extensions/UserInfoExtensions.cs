using System;
using System.Linq;
using Boilerplate.Common.Authorization;
using Boilerplate.Common.Types.Const;

namespace Boilerplate.Common.Extensions
{
    public static class UserInfoExtensions
    {
        public static bool IsAdmin(this UserInfo user) =>
            user.Roles?.Contains(RoleType.ADMIN) == true;
    }
}
