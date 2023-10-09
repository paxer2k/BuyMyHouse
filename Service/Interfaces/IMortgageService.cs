using Domain;
using Domain.DTOs;

namespace Service.Interfaces
{
    public interface IMortgageService
    {
        Task<Mortgage> CreateMortgageAsync(MortgageDTO mortgageDTO);
        Task<IEnumerable<Mortgage>> GetAllMortgagesAsync();
        Task<Mortgage> GetMortgageByIdAsync(Guid id);
        Task<Mortgage> GetMortgageByCustomerIdAsync(Guid customerId);
        Task<bool?> UpdateMortgageAsync(Mortgage mortgage);
        Task<bool> IsMortgageSent(Guid customerId);
        Task CalculateMortgageAsync();
    }
}
