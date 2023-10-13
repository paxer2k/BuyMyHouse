using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

namespace SendEmailFunction
{
    public class SendEmailTimer
    {
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;

        public SendEmailTimer(ILoggerFactory loggerFactory, IEmailService emailService)
        {
            _logger = loggerFactory.CreateLogger<SendEmailTimer>();
            _emailService = emailService;
        }

        [Function("SendEmailTimer")]
        public async Task Run([TimerTrigger("0 7 * * *")] Timer timer) // 7AM everyday
        {
            try
            {
                _logger.LogInformation("Sending mails to all customers");
                await _emailService.SendEmails();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong while sending the emails: {ex.Message}");
            }
        }
    }
}
