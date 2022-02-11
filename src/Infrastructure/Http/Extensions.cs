using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Http
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
