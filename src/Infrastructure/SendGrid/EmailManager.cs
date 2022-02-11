using System.Net;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.SendGrid
{
    public class EmailManager : IEmailManager
    {
        private readonly ISendGridClient _sendGridClient;

        public EmailManager(ISendGridClient sendGridClient)
        {
            _sendGridClient = sendGridClient;
        }

        public async Task<HttpStatusCode> SendEmail(EmailToSend emailToSend)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(emailToSend.FromEmail, emailToSend.FromName),
                Subject = emailToSend.Subject
            };
            msg.AddContent(MimeType.Html, emailToSend.Body);

            emailToSend.Recipients.ForEach(x => msg.AddTo(new EmailAddress(x.ToEmail, x.ToName)));
            var response = await _sendGridClient.SendEmailAsync(msg).ConfigureAwait(false);
            return response.StatusCode;
        }
    }
}
