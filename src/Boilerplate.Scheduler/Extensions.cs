using Boilerplate.Common.MassTransit;
using Boilerplate.Scheduler.CronScheduler;
using Boilerplate.Scheduler.ScheduledTasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Scheduler
{
    public static class Extensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MassTransitOptions>(configuration.GetSection("MassTransit"));
            services.Configure<RabbitMqQueues>(configuration.GetSection("RabbitMqQueues"));

            // Add scheduled tasks & scheduler
            services.AddCronJob<PurgeTempFilesTask>(c => c.CronExpression = CronStrings.EveryMidnight);
            services.AddCronJob<UpdateInsuranceStatusesTask>(c => c.CronExpression = "0 * * * *");

            return services;
        }
    }

    internal sealed class CronStrings
    {
        // https://crontab.guru/

        public const string EveryMinute = "* * * * *";
        public const string Every5Minute = "*/5 * * * *";
        public const string EveryHour = "0 * * * *";
        public const string EveryFourHours = "0 */4 * * *";
        public const string EveryMidnight = "0 0 * * *";
        public const string EveryThreeMonthsAtMidnight = "0 0 1 */3 *";
        public const string EveryMorning6am = "0 6 * * *";
    }
}
