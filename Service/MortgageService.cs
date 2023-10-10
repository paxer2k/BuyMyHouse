using AutoMapper;
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
        public MortgageService(IRepository<Mortgage> mortgageRepository, IRepository<Customer> customerRepository, IMapper mapper)
        {
            _mortgageRepository = mortgageRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<Mortgage> CreateMortgageAsync(MortgageDTO mortgageDTO)
        {
            if (mortgageDTO == null)
                throw new BadRequestException("The mortage could not be created.");

            if (mortgageDTO.Customers == null || mortgageDTO.Customers.Count <= 0)
                throw new BadRequestException("Oh nonononono");


            Mortgage newMortgage = new Mortgage()
            {
                PartitionKey = "1",
                Customers = new List<Customer>()
            };

            foreach (var customer in mortgageDTO.Customers)
            {
                if (CalculateAge(customer.DateOfBirth) < 18)
                    throw new BadRequestException("Sorry, but you are not eligeable for a mortage as it is 18+ only.");

                if (customer.AnualIncome < customer.MIN_INCOME)
                    throw new BadRequestException($"Sorry, you need to earn at least ${customer.MIN_INCOME} per year in order to sign up for a mortgage");

                Customer newCustomer = new Customer
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    DateOfBirth = customer.DateOfBirth,
                    AnualIncome = customer.AnualIncome,
                    PhoneNumber = customer.PhoneNumber,
                    PartitionKey = "1",
                };

                newMortgage.Customers.Add(newCustomer);
            }

            return await _mortgageRepository.CreateAsync(newMortgage);
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

        public async Task CalculateMortgageAsync()
        {
            const double INTEREST_RATE = 4.5;

            var mortgagesOfToday = await _mortgageRepository.GetAllByConditionAsync(m => m.CreatedAt == DateTime.Today);

            foreach (var mortgage in mortgagesOfToday)
            {
                var totalIncome = mortgage.Customers.Select(c => c.AnualIncome).Sum();

                mortgage.MortgageAmount += (totalIncome * INTEREST_RATE);

                await _mortgageRepository.UpdateAsync(mortgage);
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

        public async Task<IEnumerable<Mortgage>> GetAllActiveMortgages()
        {
            return await _mortgageRepository.GetAllByConditionAsync(m => m.CreatedAt == DateTime.Today.AddHours(-24)); // this action will happen on next day
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
