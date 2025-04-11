using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using TatBlog.Core.Settings;

namespace TatBlog.Services.Blogs
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendMailAsync(string subject, string body, CancellationToken cancellationToken = default)
        {
            var email = new MailMessage()
            {
                From = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            email.To.Add(_mailSettings.AdminEmail);

            using var smtp = new SmtpClient(_mailSettings.Host, _mailSettings.Port)
            {
                Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(email);
        }
    }
}
