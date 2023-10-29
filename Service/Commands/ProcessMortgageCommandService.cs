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
            var processedMortgages = await _mortgageQueryService.GetMortgagesByStatusAsync(ApplicationStatus.Processed);

            foreach(var mortgage in processedMortgages)
            {
                await ProcessMortgageAsync(mortgage);
            }
        }

        private async Task ProcessMortgageAsync(Mortgage mortgage)
        {
            var ageBelowMinimum = mortgage.Customers.Select(c => CalculateAge(c.DateOfBirth) < _appConfiguration.BusinessLogicConfig.MIN_AGE).Any();
            var totalIncome = mortgage.Customers.Select(c => c.AnualIncome).Sum();

            if (totalIncome < _appConfiguration.BusinessLogicConfig.MIN_INCOME)
                mortgage.ApplicationStatus = ApplicationStatus.Declined;
            else if (ageBelowMinimum)
                mortgage.ApplicationStatus = ApplicationStatus.Declined;
            else
                mortgage.ApplicationStatus = ApplicationStatus.Approved;

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
