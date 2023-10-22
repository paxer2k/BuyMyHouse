using Domain;
using Domain.Configuration.Interfaces;
using Service.Commands.Interfaces;
using Service.Queries.Interfaces;

namespace Service.Commands
{
    public class CalculateMortgageCommandService : ICalculateMortgageCommandService
    {
        private readonly IMortgageCommandService _mortgageCommand;
        private readonly IMortgageQueryService _mortgageQuery;
        private readonly IAppConfiguration _appConfiguration;
        public CalculateMortgageCommandService(IMortgageCommandService mortgageCommand, IMortgageQueryService mortgageQuery, IAppConfiguration appConfiguration)
        {
            _mortgageCommand = mortgageCommand;
            _mortgageQuery = mortgageQuery;
            _appConfiguration = appConfiguration;
        }
        public async Task CalculateMortgagesAsync()
        {
            var activeMortgagesOfYesterday = await _mortgageQuery.GetActiveMortgagesOfYesterday();

            await CalculateAllMortgagesAsync(activeMortgagesOfYesterday);
        }

        private async Task CalculateAllMortgagesAsync(IEnumerable<Mortgage> activeMortgagesOfYesterday)
        {
            foreach (var mortgage in activeMortgagesOfYesterday)
            {
                var totalIncome = mortgage.Customers.Select(c => c.AnualIncome).Sum();

                mortgage.MortgageAmount += (totalIncome * _appConfiguration.BusinessLogicConfig.INTEREST_RATE);

                if (mortgage.MortgageAmount > 0) // if its changed
                    mortgage.IsApproved = true;

                await _mortgageCommand.UpdateMortgageAsync(mortgage);
            }
        }
    }
}
