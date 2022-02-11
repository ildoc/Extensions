using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Ldap
{
    public static class LdapExtensions
    {
        public static IServiceCollection AddLdapAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<LdapSettings>(configuration.GetSection("LdapSettings"));
            services.AddTransient<ILdapAuthenticationService, LdapAuthenticationService>();
            return services;
        }
    }
}
