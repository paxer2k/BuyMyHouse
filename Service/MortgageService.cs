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
                throw new BadRequestException($"The mortage with id {id} does not exist!");

            return mortgage;
        }
    }
}
