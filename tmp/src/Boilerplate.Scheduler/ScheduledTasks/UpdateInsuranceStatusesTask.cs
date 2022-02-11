using System;
using System.Threading;
using System.Threading.Tasks;
using Boilerplate.Common.MassTransit;
using Boilerplate.Common.MassTransit.Commands;
using Boilerplate.Scheduler.CronScheduler;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Boilerplate.Scheduler.ScheduledTasks
{
    public class UpdateInsuranceStatusesTask : CronJobService
    {
        private readonly ILogger _logger;
        private readonly MassTransitOptions _masstransitOptions;
        private readonly RabbitMqQueues _rabbitMqQueues;
        private readonly IBus _bus;

        public UpdateInsuranceStatusesTask(IOptions<MassTransitOptions> masstransitOptions, IOptions<RabbitMqQueues> rabbitMqQueues, IBus bus,
            IScheduleConfig<UpdateInsuranceStatusesTask> config, ILogger<UpdateInsuranceStatusesTask> logger)
             : base(config, logger)
        {
            _logger = logger;
            _masstransitOptions = masstransitOptions.Value;
            _rabbitMqQueues = rabbitMqQueues.Value;
            _bus = bus;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateInsuranceStatusesTask started => {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            return base.StartAsync(cancellationToken);
        }

        public override async Task ExecuteTask(CancellationToken cancellationToken)
        {
            var client = _bus.CreateRequestClient<IUpdateInsuranceStatusesTaskCommand>(new Uri($"{_masstransitOptions.ServerUri}/{_rabbitMqQueues.Api}"));
            var response = await client.GetResponse<IUpdateInsuranceStatusesTaskResult>(new { });

            _logger.LogInformation($"UpdateInsuranceStatusesTask ended with result {response.Message.Result} => {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateInsuranceStatusesTask stopped => {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
            return base.StopAsync(cancellationToken);
        }
    }
}
