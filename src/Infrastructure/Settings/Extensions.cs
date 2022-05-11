using Infrastructure.Types.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Settings
{
    public static class Extensions
    {
        public static IServiceCollection AddApplicationSettings<T>(this IServiceCollection services) where T : DbContext, IDbContextWithSettings
        {
            services.AddTransient<IApplicationSettingsManager, ApplicationSettingsManager<T>>();

            return services;
        }
    }
}
