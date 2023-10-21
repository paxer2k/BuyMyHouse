using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Service.Command.Interfaces;

namespace SendEmailFunction
{
    public class SendEmailTimer
    {
        private readonly ILogger _logger;
        private readonly IEmailCommandService _emailCommandService;

        public SendEmailTimer(ILoggerFactory loggerFactory, IEmailCommandService emailCommandService)
        {
            _logger = loggerFactory.CreateLogger<SendEmailTimer>();
            _emailCommandService = emailCommandService;
        }

        [Function("SendEmailTimer")]
        public async Task Run([TimerTrigger("0 7 * * *")] MyInfo myInfo) // 7AM everyday send an email
        {
            try
            {
                _logger.LogInformation("Sending mails to all customers");
                await _emailCommandService.SendEmailsAsync();
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
