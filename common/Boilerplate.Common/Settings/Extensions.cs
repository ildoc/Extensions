using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Common.Settings
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
