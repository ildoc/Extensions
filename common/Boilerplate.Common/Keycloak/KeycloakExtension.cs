using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Boilerplate.Common.Keycloak;

namespace Boilerplate.Common.Keycloak
{
    public static class KeycloakExtension
    {
        public static IServiceCollection AddKeycloackClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<KeycloakSettings>(configuration.GetSection("AuthSettings"));
            services.AddHttpClient();
            services.AddTransient<IKeycloakClient, KeycloakClient>();
            return services;
        }
    }
}
