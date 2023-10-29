using Domain;
using Domain.Configuration.Interfaces;
using Service.Commands.Interfaces;
using Service.Queries.Interfaces;
using Domain.Enums;

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
            var activeMortgages = await _mortgageQuery.GetMortgagesByStatusAsync(ApplicationStatus.Active);

            foreach (var mortgage in activeMortgages)
            {
                await CalculateMortgageAsync(mortgage);
            }
        }

        private async Task CalculateMortgageAsync(Mortgage mortgage)
        {
            var totalIncome = mortgage.Customers.Select(c => c.AnualIncome).Sum();

            mortgage.MortgageAmount += (totalIncome * _appConfiguration.BusinessLogicConfig.INTEREST_RATE);

            mortgage.ApplicationStatus = ApplicationStatus.Calculated;

            await _mortgageCommand.UpdateMortgageAsync(mortgage);
        }
    }
}
