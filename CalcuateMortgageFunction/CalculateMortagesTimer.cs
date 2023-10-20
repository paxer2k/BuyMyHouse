using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Service.Command.Interfaces;

namespace CalcuateMortgageFunction
{
    public class CalculateMortagesTimer
    {
        private readonly ILogger _logger;
        private readonly ICalculateMortgageCommandService _calculateMortgageCommandService;

        public CalculateMortagesTimer(ILoggerFactory loggerFactory, ICalculateMortgageCommandService calculateMortgageCommandService)
        {
            _logger = loggerFactory.CreateLogger<CalculateMortagesTimer>();
            _calculateMortgageCommandService = calculateMortgageCommandService;
        }

        [Function("CalculateMortgagesTimer")]
        public async Task Run([TimerTrigger("* * * * *")] MyInfo myInfo) // at 11:59:59 PM EACH DAY | 59 23 * * *
        {
            try
            {
                _logger.LogInformation($"Calculating mortgages for all applications...");
                await _calculateMortgageCommandService.CalculateMortgagesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong while calculating mortgages: {ex.Message}");
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
