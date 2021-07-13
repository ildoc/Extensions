using Boilerplate.Common.MassTransit;
using Boilerplate.Common.Types.Base;
using Boilerplate.Common.Types.Interfaces;
using Boilerplate.Notifications.Managers;
using Boilerplate.Notifications.Managers.Interfaces;
using Boilerplate.Notifications.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Notifications
{
    public static class Extensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NotificationContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("NotificationContext"));
                options.EnableSensitiveDataLogging(true);
            });

            services.Configure<MassTransitOptions>(configuration.GetSection("MassTransit"));

            services.AddScoped<IDbContextWithSettings>(provider => provider.GetRequiredService<NotificationContext>());
            services.AddTransient<NotificationContextSeed>();

            services.Configure<RabbitMqQueues>(configuration.GetSection("RabbitMqQueues"));

            // Repository
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            // Managers
            services.AddTransient<INotificationManager, NotificationManager>();
            services.AddTransient<ISendNotificationManager, SendNotificationManager>();

            services.AddSingleton<InMemoryStore>();
            return services;
        }
    }
}
