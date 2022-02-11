using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infrastructure.WebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class NoHumanUser : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Claims.Any()) // un po' deboluccio...
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
