using System;

namespace Microservices.Common.MassTransit.Messages
{
    public interface IScheduledTask
    {
        DateTime Date { get; set; }
    }
}
