using System;

namespace Boilerplate.Common.MassTransit.Messages
{
    public interface IScheduledTask
    {
        DateTime Date { get; set; }
    }
}
