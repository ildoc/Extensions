using System;

namespace Boilerplate.Scheduler.CronScheduler
{
    public interface IScheduleConfig
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
    }

    public interface IScheduleConfig<T> : IScheduleConfig { }

    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string CronExpression { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }
    }
}
