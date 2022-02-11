using System.Collections.Generic;

namespace Boilerplate.Common.Types.Interfaces
{
    public interface IPushNotificationMessage
    {
        IReadOnlyList<string> Recipients { get; set; }
        string Method { get; set; }
        IPushNotificationPayload Body { get; set; }
    }

    public interface IPushNotificationPayload
    {
        string Guid { get; set; }
        string Title { get; set; }
        string Description { get; set; }
    }
}
