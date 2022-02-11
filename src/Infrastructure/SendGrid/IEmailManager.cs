using System.Net;
using System.Threading.Tasks;

namespace Infrastructure.SendGrid
{
    public interface IEmailManager
    {
        Task<HttpStatusCode> SendEmail(EmailToSend emailToSend);
    }
}
