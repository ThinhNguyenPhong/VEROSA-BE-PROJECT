using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using VEROSA.Common.Models.Settings;

namespace VEROSA.BussinessLogicLayer.Services.Email
{
    public class MailKitEmailService : IEmailService
    {
        private readonly SmtpSettings _opts;

        public MailKitEmailService(IOptions<SmtpSettings> opts) => _opts = opts.Value;

        public async Task SendEmailAsync(string to, string subject, string htmlBody)
        {
            var msg = new MimeMessage();
            msg.From.Add(MailboxAddress.Parse(_opts.From));
            msg.To.Add(MailboxAddress.Parse(to));
            msg.Subject = subject;
            msg.Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody();

            using var client = new SmtpClient();

            SecureSocketOptions socketOptions;
            if (_opts.Port == 465)
                socketOptions = SecureSocketOptions.SslOnConnect;
            else if (_opts.Port == 587)
                socketOptions = SecureSocketOptions.StartTls;
            else
                socketOptions = SecureSocketOptions.Auto;

            await client.ConnectAsync(_opts.Host, _opts.Port, socketOptions);
            await client.AuthenticateAsync(_opts.Username, _opts.Password);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
    }
}
