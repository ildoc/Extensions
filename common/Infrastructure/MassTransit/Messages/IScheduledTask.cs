using System;

namespace Infrastructure.MassTransit.Messages
{
    public interface IScheduledTask
    {
        DateTime Date { get; set; }
    }
}
