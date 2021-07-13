using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.Health
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomHealthCheck(this IServiceCollection services, IConfiguration configuration, string connectionStringVariable)
        {
            var hcBuilder = services.AddHealthChecks();

            var serviceName = configuration["App:Name"];

            hcBuilder
                .AddSqlServer(
                    configuration.GetConnectionString(connectionStringVariable),
                    name: $"{serviceName}-DB-check",
                    tags: new string[] { serviceName.ToLower() });

            return services;
        }

        public static IEndpointRouteBuilder MapCustomHealthChecks(this IEndpointRouteBuilder endpoint)
        {
            endpoint.MapHealthChecks("/hc", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return endpoint;
        }
    }
}
