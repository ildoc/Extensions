using System.Net;
using System.Threading.Tasks;

namespace Microservices.Common.SendGrid
{
    public interface IEmailManager
    {
        Task<HttpStatusCode> SendEmail(EmailToSend emailToSend);
    }
}