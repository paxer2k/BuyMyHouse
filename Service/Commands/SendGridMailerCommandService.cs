using Domain.Configuration.Interfaces;
using SendGrid.Helpers.Mail;
using SendGrid;
using Service.Commands.Interfaces;

namespace Service.Commands
{
    public class SendGridMailerCommandService : ISendGridMailerCommandService
    {
        private IAppConfiguration _appConfiguration;
        private SendGridClient _sendGridClient;

        public SendGridMailerCommandService(IAppConfiguration appConfiguration)
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
