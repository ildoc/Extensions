using System.Net;
using System.Threading.Tasks;

namespace Boilerplate.Common.SendGrid
{
    public interface IEmailManager
    {
        Task<HttpStatusCode> SendEmail(EmailToSend emailToSend);
    }
}