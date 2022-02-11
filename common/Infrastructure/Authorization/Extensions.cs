using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Infrastructure.Types.Const;
using Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Authorization
{
    public static class Extensions
    {
        public static IServiceCollection AddJwtAuthorization(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;

                o.Authority = $"{configuration["AuthSettings:KeycloakBaseUrl"]}realms/{configuration["AuthSettings:RealmName"]}";
                o.Audience = configuration["AuthSettings:WebAppClientId"];

                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();

                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        if (isDevelopment)
                        {
                            return c.Response.WriteAsync(c.Exception.ToString());
                        }
                        return c.Response.WriteAsync("An error occured processing your authentication.");
                    }
                };
            });

            return services;
        }

        public static UserInfo GetUserInfo(this ClaimsPrincipal user) => InitializeUserInfo(user.Claims);
        public static UserInfo GetUserInfo(this IEnumerable<Claim> claims) => InitializeUserInfo(claims);

        private static UserInfo InitializeUserInfo(IEnumerable<Claim> claims)
        {
            return new UserInfo
            {
                UserId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                TaxCode = claims.FirstOrDefault(c => c.Type == ClaimsNames.PREFERRED_USERNAME)?.Value ?? "",
                IsPhysicalPerson = claims.FirstOrDefault(c => c.Type == ClaimsNames.IS_PHYSICAL_PERSON)?.Value.ToBool() ?? true,
                Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? "",
                Roles = claims.Where(c => c.Type == ClaimTypes.Role)?.Select(x => x.Value).ToList(),
                IsImpersonated = !claims.Any(c => c.Type == ClaimsNames.AUTH_TIME),
                //FullName = claims.FirstOrDefault(c => c.Type == ClaimsNames.NAME)?.Value ?? "";
                //Name = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? "";
                //Surname = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? "";
                //SubscriptionId = Convert.ToInt32(claims.FirstOrDefault(c => c.Type == ClaimsNames.SUBSCRIPTION_ID)?.Value ?? "0");
                //SubscriptionName = claims.FirstOrDefault(c => c.Type == ClaimsNames.SUBSCRIPTION_NAME)?.Value ?? "";
                //SubscriptionIsManaged = claims.FirstOrDefault(c => c.Type == ClaimsNames.SUB_IS_MANAGED)?.Value == "True";
                //SubscriptionFunctionalities = functionalities.IsNullOrEmpty() ? Array.Empty<string>() : functionalities.Split(";");
            };
        }
    }
}
