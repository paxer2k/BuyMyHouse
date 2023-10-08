using AutoMapper;
using DAL.Repository.Interfaces;
using Domain;
using Domain.DTOs;
using Service.Exceptions;
using Service.Interfaces;
using System.Security.Cryptography.X509Certificates;

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

            var customer = await _customerRepository.GetByConditionAsync(c => c.Id == mortgageDTO.CustomerId);

            if (customer == null)
                throw new NotFoundException("The customer with the given id could not be found!");

            /*if (mortgageApplicationDTO.AnualIncome <= 0)
                throw new BadRequestException("Proper anual income has to be added (0 or more)");*/

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

        public async Task<bool> HasSentApplication(Guid customerId)
        {
            var mortgage = await _mortgageRepository.GetByConditionAsync(m => m.CustomerId == customerId);

            return mortgage != null;
        }

        public async Task CalculateMortgage()
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
    }
}
