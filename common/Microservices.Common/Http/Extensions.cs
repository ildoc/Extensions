using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.Http
{
    public static class Extensions
    {
        public static IServiceCollection AddHttpManager(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IHttpManager, HttpManager>();
            return services;
        }
    }
}
