using Domain;
using Domain.DTOs;
using Domain.Overview;

namespace Service.Interfaces
{
    public interface IMortgageService
    {
        Task<MortgageDTO> CreateMortgageAsync(MortgageDTO mortgageDTO);
        Task<GenericOverview<Mortgage>> GetAllMortgagesAsync(int startIndex, int endIndex);
        Task<Mortgage> GetMortgageByIdAsync(Guid id);
        Task<IEnumerable<Mortgage>> GetAllActiveMortgages();
        Task<bool?> UpdateMortgageAsync(Mortgage mortgage);
    }
}
