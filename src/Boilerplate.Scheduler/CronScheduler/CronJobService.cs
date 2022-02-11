using System;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace Boilerplate.Scheduler.CronScheduler
{
    public abstract class CronJobService : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly ILogger _logger;

        protected CronJobService(IScheduleConfig config, ILogger logger)
        {
            _expression = CronExpression.Parse(config.CronExpression);
            _timeZoneInfo = config.TimeZoneInfo ?? TimeZoneInfo.Local;
            _logger = logger;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJob(cancellationToken);
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
                {
                    await ScheduleJob(cancellationToken);
                }

                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose(); 
                    _timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        var policy = GetPolicy();

                        var result = await policy.ExecuteAndCaptureAsync(async () =>
                        {
                            await ExecuteTask(cancellationToken);
                        });
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {

                        await ScheduleJob(cancellationToken);
                    }
                };
                _timer.Start();
            }
            await Task.CompletedTask;
        }

        private AsyncRetryPolicy GetPolicy(int retries = 3)
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                     retryCount: retries,
                     sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                     onRetry: (exception, timeSpan, retry, ctx) =>
                     {
                         _logger.LogWarning(
                             exception,
                             "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                             nameof(CronJobService),
                             exception.GetType().Name,
                             exception.Message,
                             retry,
                             retries
                             );
                     }
                 );
        }

        public virtual async Task ExecuteTask(CancellationToken cancellationToken)
        {
            await Task.Delay(5000, cancellationToken);  // do the work
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
