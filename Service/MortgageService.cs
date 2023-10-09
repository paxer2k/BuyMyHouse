using AutoMapper;
using DAL;
using DAL.Repository.Interfaces;
using Domain;
using Domain.DTOs;
using Service.Exceptions;
using Service.Interfaces;

namespace Service
{
    public class MortgageService : IMortgageService
    {
        private readonly IRepository<Mortgage> _mortgageRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;
        public MortgageService(IRepository<Mortgage> mortgageRepository, IRepository<Customer> customerRepository, IMapper mapper, AppConfiguration appConfiguration)
        {
            _mortgageRepository = mortgageRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<Mortgage> CreateMortgageAsync(MortgageDTO mortgageDTO)
        {
            if (mortgageDTO == null)
                throw new BadRequestException("The mortage could not be created.");

            var customer = await _customerRepository.GetByConditionAsync(c => c.Id == mortgageDTO.CustomerId);

            if (customer == null)
                throw new NotFoundException("The customer with the given id could not be found!");

            if (CalculateAge(customer.DateOfBirth) < 18)
                throw new BadRequestException("Sorry, but you are not eligeable for a mortage as it is 18+ only.");

            if (customer.AnualIncome < customer.MIN_INCOME)
                throw new BadRequestException($"Sorry, you need to earn at least ${customer.MIN_INCOME} per year in order to sign up for a mortgage");

            return await _mortgageRepository.CreateAsync(_mapper.Map<Mortgage>(mortgageDTO));
        }

        public async Task<IEnumerable<Mortgage>> GetAllMortgagesAsync()
        {
            var mortgages = await _mortgageRepository.GetAllAsync();

            if (mortgages == null)
                throw new BadRequestException("The mortages could not be retrieved");

            return mortgages;
        }

        public async Task<Mortgage> GetMortgageByIdAsync(Guid id)
        {
            var mortgage = await _mortgageRepository.GetByConditionAsync(m => m.Id == id);

            if (mortgage == null)
                throw new BadRequestException($"The mortgage with id {id} does not exist!");

            return mortgage;
        }

        public async Task<Mortgage> GetMortgageByCustomerIdAsync(Guid customerId)
        {
            var mortgage = await _mortgageRepository.GetByConditionAsync(m => m.CustomerId == customerId);

            if (mortgage == null)
                throw new BadRequestException("No mortgage available for this user!");

            if (mortgage.ExpiresAt < DateTime.Now)
                throw new BadRequestException($"Sorry, the possibility to view this mortage has expired");

            return mortgage;
        }

        public async Task<bool> IsMortgageSent(Guid customerId)
        {
            var mortgage = await _mortgageRepository.GetByConditionAsync(m => m.CustomerId == customerId);

            return mortgage != null;
        }

        public async Task CalculateMortgageAsync()
        {
            const double INTEREST_RATE = 4.5;

            var customers = await _customerRepository.GetAllAsync();

            foreach (var customer in customers)
            {
                var mortgage = await _mortgageRepository.GetByConditionAsync(m => m.CustomerId == customer.Id);

                if (mortgage == null) // if mortgage was not filled out, ignore, otherwise set mortgage amount
                    continue;

                mortgage.MortgageAmount += (customer.AnualIncome * INTEREST_RATE);
            }
        }

        public async Task<bool?> UpdateMortgageAsync(Mortgage mortgage)
        {
            if (mortgage == null)
                throw new BadRequestException("The mortgage object was not initialized...");

            if (string.IsNullOrEmpty(mortgage.Id.ToString()))
                throw new BadRequestException("The mortgage does not exist in the database");

            return await _mortgageRepository.UpdateAsync(mortgage);
        }

        private int CalculateAge(DateOnly birthDate)
        {
            DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

            int age = currentDate.Year - birthDate.Year;

            if (currentDate.DayOfYear < birthDate.DayOfYear)
                age--;

            return age;
        }
    }
}
