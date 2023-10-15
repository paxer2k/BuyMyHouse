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
        public async Task Run([TimerTrigger("* * * * *")] MyInfo myInfo) // 7AM everyday 0 7 * * *
        {
            try
            {
                _logger.LogInformation("Sending mails to all customers");
                await _emailService.SendEmailsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong while sending the emails: {ex.Message}");
            }
        }

        public class MyInfo
        {
            public MyScheduleStatus ScheduleStatus { get; set; }
            public bool IsPastDue { get; set; }
        }

        public class MyScheduleStatus
        {
            public DateTime Last { get; set; }
            public DateTime Next { get; set; }
            public DateTime LastUpdated { get; set; }
        }
    }
}
