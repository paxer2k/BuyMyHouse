using Domain;
using Domain.DTOs;
using Domain.Enums;
using Domain.Overview;

namespace Service.Queries.Interfaces
{
    public interface IMortgageQueryService
    {
        Task<GenericOverview<MortgageResponseDTO>> GetAllMortgagesAsync(int startIndex = 0, int endIndex = 9);
        Task<MortgageResponseDTO> GetMortgageByIdAsync(Guid id);
        Task<IEnumerable<Mortgage>> GetMortgagesByStatusAsync(MortgageStatus mortgageStatus);
        Task<IEnumerable<Mortgage>> GetProcessedMortgages();
    }
}