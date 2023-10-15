using Domain.Configuration.Interfaces;
using Service.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Service
{
    public class SmtpClientMailer : ISmtpClientMailer
    {
        private readonly IAppConfiguration _appConfiguration;
        private readonly SmtpClient _smtpClient;

        public SmtpClientMailer(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;

            _smtpClient = new SmtpClient(_appConfiguration.SmtpConfig.Server, _appConfiguration.SmtpConfig.Port);
            _smtpClient.EnableSsl = true;
            _smtpClient.Credentials = new NetworkCredential(_appConfiguration.SmtpConfig.Sender, _appConfiguration.SmtpConfig.Password);
            _smtpClient.UseDefaultCredentials = false;
        }

        public async Task SendEmailAsync(MailMessage message)
        {
            await _smtpClient.SendMailAsync(message);
        }
    }
}
