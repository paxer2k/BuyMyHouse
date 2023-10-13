using DAL.Configuration.Interfaces;
using DAL.Repository.Interfaces;
using Domain;
using Service.Interfaces;

namespace Service
{
    public class MortgageCalculatorService : IMortgageCalculatorService
    {
        private readonly IRepository<Mortgage> _mortgageRepository;
        private readonly IAppConfiguration _appConfiguration;

        public MortgageCalculatorService(IRepository<Mortgage> mortgageRepository, IAppConfiguration appConfiguration)
        {
            _mortgageRepository = mortgageRepository;
            _appConfiguration = appConfiguration;
        }
        /// <summary>
        /// Function responsible of calculating all of the mortgages for that day.
        /// </summary>
        /// <returns></returns>
        public async Task CalculateMortgagesAsync()
        {
            var mortgagesOfToday = await _mortgageRepository.GetAllByConditionAsync(m => m.CreatedAt == DateTime.Today);

            foreach (var mortgage in mortgagesOfToday)
            {
                var totalIncome = mortgage.Customers.Select(c => c.AnualIncome).Sum();

                mortgage.MortgageAmount += (totalIncome * _appConfiguration.BusinessLogicConfig.INTEREST_RATE);

                await _mortgageRepository.UpdateAsync(mortgage);
            }
        }
    }
}
