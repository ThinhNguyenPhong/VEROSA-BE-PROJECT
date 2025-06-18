using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using VEROSA.Common.Models.JWTSettings;

namespace VEROSA.BussinessLogicLayer.Services.Email
{
    public class SmtpEmailService : IEmailService
    {
        private readonly SmtpSettings _opts;

        public SmtpEmailService(IOptions<SmtpSettings> opts) => _opts = opts.Value;

        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            using var client = new SmtpClient(_opts.Host, _opts.Port)
            {
                Credentials = new NetworkCredential(_opts.Username, _opts.Password),
                EnableSsl = _opts.EnableSsl,
            };

            var mail = new MailMessage(_opts.From, to, subject, htmlBody) { IsBodyHtml = true };

            await client.SendMailAsync(mail);
        }
    }
}
