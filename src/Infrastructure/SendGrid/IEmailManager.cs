using System.Net;

namespace Infrastructure.SendGrid
{
    public interface IEmailManager
    {
        Task<HttpStatusCode> SendEmail(EmailToSend emailToSend);
    }
}
