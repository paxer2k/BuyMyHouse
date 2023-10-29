using Domain;
using Domain.Configuration.Interfaces;
using Domain.Enums;
using Service.Commands.Interfaces;
using Service.Queries.Interfaces;

namespace Service.Commands
{
    public class ProcessMortgageCommandService : IProcessMortgageCommandService
    {
        private readonly IMortgageCommandService _mortgageCommandService;
        private readonly IMortgageQueryService _mortgageQueryService;
        private readonly IAppConfiguration _appConfiguration;

        public ProcessMortgageCommandService(IMortgageCommandService mortgageCommandService, IMortgageQueryService mortgageQuery, IAppConfiguration appConfiguration)
        {
            _mortgageCommandService = mortgageCommandService;
            _mortgageQueryService = mortgageQuery;
            _appConfiguration = appConfiguration;
        }
        public async Task ProcessMortgagesAsync()
        {
            var calculatedMortgages = await _mortgageQueryService.GetMortgagesByStatusAsync(MortgageStatus.Calculated);

            foreach(var mortgage in calculatedMortgages)
            {
                await ProcessMortgageAsync(mortgage);
            }
        }

        private async Task ProcessMortgageAsync(Mortgage mortgage)
        {
            var ageBelowMinimum = mortgage.Customers
                .Select(c => CalculateAge(c.DateOfBirth))
                .Any(age => age < _appConfiguration.BusinessLogicConfig.MIN_AGE);

            var totalIncome = mortgage.Customers.Select(c => c.AnualIncome).Sum();

            if (totalIncome < _appConfiguration.BusinessLogicConfig.MIN_INCOME)
                mortgage.MortgageStatus = MortgageStatus.Declined;
            else if (ageBelowMinimum)
                mortgage.MortgageStatus = MortgageStatus.Declined;
            else
                mortgage.MortgageStatus = MortgageStatus.Approved;

            await _mortgageCommandService.UpdateMortgageAsync(mortgage);
        }

        private int CalculateAge(string birthDate)
        {
            DateTime birthdate = DateTime.Parse(birthDate);

            DateTime currentDate = DateTime.Today;
            int age = currentDate.Year - birthdate.Year;

            // Check if the birthday for this year has already occurred or not
            if (birthdate.Date > currentDate.AddYears(-age))
                age--;

            return age;
        }     
    }
}
