using Domain;
using Domain.DTOs;

namespace Service.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(Guid id);
        Task<Customer> CreateCustomer(CustomerDTO userDTO);
    }
}
