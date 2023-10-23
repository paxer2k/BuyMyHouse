using Domain;
using Domain.DTOs;
using Domain.Overview;

namespace Service.Queries.Interfaces
{
    public interface IMortgageQueryService
    {
        Task<GenericOverview<MortgageResponseDTO>> GetAllMortgagesAsync(int startIndex = 0, int endIndex = 9);
        Task<MortgageResponseDTO> GetMortgageByIdAsync(Guid id);
        Task<IEnumerable<Mortgage>> GetActiveMortgagesOfYesterdayAsync();
        Task<IEnumerable<Mortgage>> GetApprovedMortgagesOfYesterdayAsync();
    }
}
