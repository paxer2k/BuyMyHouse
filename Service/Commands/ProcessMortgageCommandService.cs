using Domain;
using Domain.Configuration.Interfaces;
using Domain.Enums;
using Service.Commands.Interfaces;
using Service.Queries.Interfaces;
using System.Globalization;

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
            var processedApplications = await _mortgageQueryService.GetMortgagesByStatusAsync(ApplicationStatus.Processed);

            foreach(var application in processedApplications)
            {
                foreach(var customer in application.Customers)
                {
                    await ProcessMortgageAsync(application, customer);
                }
            }
        }

        private async Task ProcessMortgageAsync(Mortgage mortgage, Customer customer)
        {
            if (CalculateAge(customer.DateOfBirth) < _appConfiguration.BusinessLogicConfig.MIN_AGE)
                mortgage.ApplicationStatus = ApplicationStatus.Declined;

            if (customer.AnualIncome < _appConfiguration.BusinessLogicConfig.MIN_INCOME)
                mortgage.ApplicationStatus = ApplicationStatus.Declined;

            mortgage.ApplicationStatus = ApplicationStatus.Approved;        
            
            await _mortgageCommandService.UpdateMortgageAsync(mortgage);
        }

        private int CalculateAge(string birthDate)
        {
            DateTime birthdate;
            if (!DateTime.TryParseExact(birthDate, "yyyy-MM-dd", null, DateTimeStyles.None, out birthdate))
                throw new ArgumentException("Invalid birthdate format. Please use 'yyyy-MM-dd'.");

            DateTime currentDate = DateTime.Today;
            int age = currentDate.Year - birthdate.Year;

            // Check if the birthday for this year has already occurred or not
            if (birthdate.Date > currentDate.AddYears(-age))
                age--;

            return age;
        }     
    }
}
