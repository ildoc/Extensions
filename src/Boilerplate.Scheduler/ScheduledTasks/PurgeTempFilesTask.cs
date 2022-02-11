using System;
using System.Threading;
using System.Threading.Tasks;
using Boilerplate.Common.MassTransit;
using Boilerplate.Common.MassTransit.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Boilerplate.Scheduler.CronScheduler;

namespace Boilerplate.Scheduler.ScheduledTasks
{
    public class PurgeTempFilesTask : CronJobService
    {
        private readonly ILogger _logger;
        private readonly MassTransitOptions _masstransitOptions;
        private readonly RabbitMqQueues _rabbitMqQueues;
        private readonly IBus _bus;

        public PurgeTempFilesTask(IOptions<MassTransitOptions> masstransitOptions, IOptions<RabbitMqQueues> rabbitMqQueues, IBus bus,
            IScheduleConfig<PurgeTempFilesTask> config, ILogger<PurgeTempFilesTask> logger)
             : base(config, logger)
        {
            _logger = logger;
            _masstransitOptions = masstransitOptions.Value;
            _rabbitMqQueues = rabbitMqQueues.Value;
            _bus = bus;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"PurgeTempFilesTask started => {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            return base.StartAsync(cancellationToken);
        }

        public override async Task ExecuteTask(CancellationToken cancellationToken)
        {
            var client = _bus.CreateRequestClient<IPurgeTempFilesTaskCommand>(new Uri($"{_masstransitOptions.ServerUri}/{_rabbitMqQueues.Api}"));
            var response = await client.GetResponse<IPurgeTempFilesTaskResult>(new { });

            _logger.LogInformation($"PurgeTempFilesTask ended with result {response.Message.Result} => {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"PurgeTempFilesTask stopped => {DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")}");
            return base.StopAsync(cancellationToken);
        }
    }
}
