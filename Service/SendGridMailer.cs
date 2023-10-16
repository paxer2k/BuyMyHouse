using Domain.Configuration.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using Service.Interfaces;

namespace Service
{
    public class SendGridMailer : ISendGridMailer
    {
        private IAppConfiguration _appConfiguration;
        private SendGridClient _sendGridClient;

        public SendGridMailer(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;

            _sendGridClient = new SendGridClient(_appConfiguration.SendGridConfig.SendGridApiKey);

        }
        public async Task SendEmailAsync(SendGridMessage message)
        {
            await _sendGridClient.SendEmailAsync(message);
        }
    }
}
