using Domain;
using Domain.DTOs;
using Domain.Overview;

namespace Service.Interfaces
{
    public interface IMortgageService
    {
        Task<MortgageDTO> CreateMortgageAsync(MortgageDTO mortgageDTO);
        Task<GenericOverview<MortgageResponseDTO>> GetAllMortgagesAsync(int startIndex = 0, int endIndex = 9);
        Task<MortgageResponseDTO> GetMortgageByIdAsync(Guid id);
        Task<IEnumerable<Mortgage>> GetMortgagesOfToday();
        Task<IEnumerable<Mortgage>> GetActiveMortgagesOfYesterday();
        Task<bool?> UpdateMortgageAsync(Mortgage mortgage);
    }
}
