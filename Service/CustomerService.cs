using AutoMapper;
using DAL;
using DAL.Repository.Interfaces;
using Domain;
using Domain.DTOs;
using Service.Exceptions;
using Service.Interfaces;
using System.Globalization;

namespace Service
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(IRepository<Customer> customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            if (customers == null)
                throw new BadRequestException("The users do not exist!");

            return customers;
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid id)
        {
            var customer = await _customerRepository.GetByConditionAsync(c => c.Id == id);

            if (customer == null)
                throw new NotFoundException($"The user with id {id} was not found!");

            return customer;
        }

        public async Task<Customer> CreateCustomer(CustomerDTO customerDTO)
        {
            if (customerDTO == null)
                throw new BadRequestException("The user object has to be instanciated..");

            if (string.IsNullOrEmpty(customerDTO.FirstName) || string.IsNullOrEmpty(customerDTO.LastName) || string.IsNullOrEmpty(customerDTO.Email) || string.IsNullOrEmpty(customerDTO.DateOfBirth.ToString()))
                throw new BadRequestException("All fields must be filled out");

/*            DateTime validDateTime;
            if (!DateTime.TryParseExact(userDTO.DateOfBirth, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out validDateTime))
                throw new BadRequestException($"Invalid date was entered, please use the following format 'dd-MM-yyyy'");*/

            return await _customerRepository.CreateAsync(_mapper.Map<Customer>(customerDTO));
        }     
    }
}
