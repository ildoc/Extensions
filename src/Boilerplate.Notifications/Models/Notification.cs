using System;
using Boilerplate.Common.Types.Abstracts;
using Boilerplate.Common.Types.Enums;
using Boilerplate.Notifications.Consts;
using Boilerplate.Notifications.Enums;

namespace Boilerplate.Notifications.Models
{
    public class Notification : SystemEntity
    {
        public string Guid { get; set; }
        public string Method { get; set; } = NotificationMethods.USERNOTIFICATION;
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reciever { get; set; }
        public NotificationLevelEnum Level { get; set; } = NotificationLevelEnum.Info;
        public StatusEnum Status { get; set; } = StatusEnum.New;
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public DateTime DeadLine { get; set; }

        public string Payload { get; set; }
    }
}
