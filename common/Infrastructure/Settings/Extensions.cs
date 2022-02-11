using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Settings
{
    public static class Extensions
    {
        public static IServiceCollection AddApplicationSettings(this IServiceCollection services)
        {
            services.AddTransient<IApplicationSettingsManager, ApplicationSettingsManager>();

            return services;
        }
    }
}
