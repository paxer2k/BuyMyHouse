using Domain;
using Domain.Configuration.Interfaces;
using Service.Command.Interfaces;
using Service.Query.Interfaces;

namespace Service.Command
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
            var mortgagesOfToday = await _mortgageQuery.GetMortgagesOfToday();

            await CalculateAllMortgagesAsync(mortgagesOfToday);
        }

        private async Task CalculateAllMortgagesAsync(IEnumerable<Mortgage> mortgagesOfToday)
        {
            foreach (var mortgage in mortgagesOfToday)
            {
                var totalIncome = mortgage.Customers.Select(c => c.AnualIncome).Sum();

                mortgage.MortgageAmount += (totalIncome * _appConfiguration.BusinessLogicConfig.INTEREST_RATE);

                await _mortgageCommand.UpdateMortgageAsync(mortgage);
            }
        }
    }
}
