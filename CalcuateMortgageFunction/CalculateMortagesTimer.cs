using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Service.Interfaces;

namespace CalcuateMortgageFunction
{
    public class CalculateMortagesTimer
    {
        private readonly ILogger _logger;
        private readonly IMortgageCalculatorService _mortgageCalculatorService;

        public CalculateMortagesTimer(ILoggerFactory loggerFactory, IMortgageCalculatorService mortgageCalculatorService)
        {
            _logger = loggerFactory.CreateLogger<CalculateMortagesTimer>();
            _mortgageCalculatorService = mortgageCalculatorService;
        }

        [Function("CalculateMortgagesTimer")]
        public async Task Run([TimerTrigger("59 23 * * *")] Timer timer) // at 11:59:59 PM EACH DAY
        {
            _logger.LogInformation($"Calculating mortgages for all applications...");
            await _mortgageCalculatorService.CalculateMortgagesAsync();
        }
    }
}
