using AutoMapper;
using DAL.Configuration.Interfaces;
using DAL.Repository.Interfaces;
using Domain;
using Domain.DTOs;
using Domain.Overview;
using Service.Exceptions;
using Service.Helpers;
using Service.Interfaces;

namespace Service
{
    public class MortgageService : IMortgageService
    {
        private readonly IRepository<Mortgage> _mortgageRepository;
        private readonly IMapper _mapper;
        private readonly IAppConfiguration _appConfiguration;
        public MortgageService(IRepository<Mortgage> mortgageRepository, IMapper mapper, IAppConfiguration appConfiguration)
        {
            _mortgageRepository = mortgageRepository;
            _mapper = mapper;
            _appConfiguration = appConfiguration;
        }

        public async Task<MortgageDTO> CreateMortgageAsync(MortgageDTO mortgageDTO)
        {
            ValidateMortgageDTO(mortgageDTO);

            Mortgage newMortgage = new Mortgage()
            {
                Customers = new List<Customer>(),
                CreatedAt = DateTime.Now            
            };

            foreach (var customer in mortgageDTO.Customers)
            {
                ValidateCustomerDTO(customer);

                Customer newCustomer = new Customer
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    DateOfBirth = customer.DateOfBirth.ToString("yyyy-MM-dd"),
                    AnualIncome = customer.AnualIncome,
                    PhoneNumber = customer.PhoneNumber,
                };

                newMortgage.Customers.Add(newCustomer);
            }

            await _mortgageRepository.CreateAsync(newMortgage);

            /*return _mapper.Map<MortgageDTO>(newMortgage);*/

            return new MortgageDTO
            {
                Customers = mortgageDTO.Customers
            };
        }

        public async Task<GenericOverview<Mortgage>> GetAllMortgagesAsync(int startIndex, int endIndex)
        {
            var mortgages = await _mortgageRepository.GetAllAsync();

            if (mortgages == null)
                throw new BadRequestException("The mortages could not be retrieved");

            var mortgageOverview = new GenericOverview<Mortgage>()
            {
                Data = mortgages.Skip(startIndex).Take(endIndex - startIndex + 1).ToList(),
                Total = mortgages.Count(),
            };

            return mortgageOverview;
        }

        public async Task<Mortgage> GetMortgageByIdAsync(Guid id)
        {
            var mortgage = await _mortgageRepository.GetByConditionAsync(m => m.Id == id);

            if (mortgage == null)
                throw new BadRequestException($"The mortgage with id {id} does not exist!");

            if (mortgage.ExpiresAt < DateTime.Now)
                throw new ForbiddenException("The time for viewing this mortgage application has expired!");

            return mortgage;
        }

        /// <summary>
        /// Function responsible for updating any field within the mortgage object in the database (e.g mortgage amount, expiry date, etc..)
        /// </summary>
        /// <param name="mortgage"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public async Task<bool?> UpdateMortgageAsync(Mortgage mortgage)
        {
            if (mortgage == null)
                throw new BadRequestException("The mortgage object was not initialized...");

            return await _mortgageRepository.UpdateAsync(mortgage);
        }

        /// <summary>
        /// Method that gets mortgages of today (this is for calculation of the total mortgages (this is for the current day))
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Mortgage>> GetMortgagesOfToday()
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);

            return await _mortgageRepository.GetAllByConditionAsync(m => m.CreatedAt >= today && m.CreatedAt < tomorrow);
        }

        /// <summary>
        /// Method that gets mortgages of yesterday (this is for sending email on next day)
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Mortgage>> GetActiveMortgagesOfYesterday()
        {
            DateTime today = DateTime.Today;
            DateTime yesterday = today.AddDays(-1);

            return await _mortgageRepository.GetAllByConditionAsync(m => m.CreatedAt >= yesterday && m.CreatedAt < today);
        }

        #region Validation methods
        /// <summary>
        /// Validation methods
        /// </summary>
        /// <param name="mortgageDTO"></param>
        /// <exception cref="BadRequestException"></exception>
        private void ValidateMortgageDTO(MortgageDTO mortgageDTO)
        {
            if (mortgageDTO == null)
                throw new BadRequestException("The mortage could not be created.");

            if (mortgageDTO.Customers == null || mortgageDTO.Customers.Count <= 0)
                throw new BadRequestException("You are required to fill out the customer(s) details");

            if (mortgageDTO.Customers.Count > 2)
                throw new BadRequestException("You are not allowed to have more than 2 customers for a mortgage.");
        }

        private void ValidateCustomerDTO(CustomerDTO customerDTO)
        {
            if (string.IsNullOrEmpty(customerDTO.FirstName) || string.IsNullOrEmpty(customerDTO.LastName))
                throw new BadImageFormatException("First name and last name are mandatory to fill out.");

            if (string.IsNullOrEmpty(customerDTO.Email))
                throw new BadImageFormatException("The email is mandatory to fill out.");

            if (string.IsNullOrEmpty(customerDTO.DateOfBirth.ToString()))
                throw new BadImageFormatException("Your date of birth is required to be filled out");

            if (MortgageHelper.CalculateAge(customerDTO.DateOfBirth) < _appConfiguration.BusinessLogicConfig.MIN_AGE)
                throw new BadRequestException("Sorry, but you are not eligeable for a mortage as it is 18+ only.");

            if (customerDTO.AnualIncome < _appConfiguration.BusinessLogicConfig.MIN_INCOME)
                throw new BadRequestException($"Sorry, you need to earn at least ${_appConfiguration.BusinessLogicConfig.MIN_INCOME} per year in order to sign up for a mortgage");
        }
        #endregion
    }
}
