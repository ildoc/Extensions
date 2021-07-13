using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Common.DbPatch
{
    public static class Extensions
    {
        public static IServiceCollection AddDbPatcher<T>(this IServiceCollection services) where T : class, IPatcher
        {
            services.AddTransient<IPatcher, T>();

            return services;
        }

        public static IApplicationBuilder ApplyDbPatches(this IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var patcher = scope.ServiceProvider.GetService<IPatcher>();
            patcher.ApplyPatches().Wait();
            return app;
        }
    }
}
