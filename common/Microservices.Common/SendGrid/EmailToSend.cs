using System.Collections.Generic;

namespace Microservices.Common.SendGrid
{
    public class EmailToSend
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<(string ToEmail, string ToName)> Recipients { get; set; }
    }
}
