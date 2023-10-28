using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Service.Commands.Interfaces;

namespace ProcessMortgagesTimer
{
    public class ProcessMortgagesTimer
    {
        private readonly ILogger _logger;
        private readonly ICalculateMortgageCommandService _calculateMortgageCommandService;
        private readonly IProcessMortgageCommandService _processApplicationCommandService;

        public ProcessMortgagesTimer(ILoggerFactory loggerFactory, ICalculateMortgageCommandService calculateMortgageCommandService, IProcessMortgageCommandService processApplicationCommandService)
        {
            _logger = loggerFactory.CreateLogger<ProcessMortgagesTimer>();
            _calculateMortgageCommandService = calculateMortgageCommandService;
            _processApplicationCommandService = processApplicationCommandService;
        }

        [Function("ProcessMortgagesTimer")]
        public async Task Run([TimerTrigger("0 0 0 * *")] MyInfo myInfo) // perform calculations at & processing at midnight so that no one can be quick and dirty at the end of the day
        {
            try
            {
                _logger.LogInformation($"Processing mortgages of today: {DateTime.Now}");
                _logger.LogInformation($"Next process schedule at: {myInfo.ScheduleStatus.Next}");

                await _calculateMortgageCommandService.CalculateMortgagesAsync();

                await _processApplicationCommandService.ProcessMortgagesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong while processing mortgages: {ex.Message}");
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
