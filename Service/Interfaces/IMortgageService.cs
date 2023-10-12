using Domain;
using Domain.DTOs;

namespace Service.Interfaces
{
    public interface IMortgageService
    {
        Task<MortgageDTO> CreateMortgageAsync(MortgageDTO mortgageDTO);
        Task<IEnumerable<Mortgage>> GetAllMortgagesAsync();
        Task<Mortgage> GetMortgageByIdAsync(Guid id);
        Task<IEnumerable<Mortgage>> GetAllActiveMortgages();
        Task<bool?> UpdateMortgageAsync(Mortgage mortgage);
        Task CalculateMortgageAsync();
    }
}
