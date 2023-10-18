using Domain;
using Domain.DTOs;

namespace Service.Interfaces
{
    public interface IMortgageCommand
    {
        Task<MortgageDTO> CreateMortgageAsync(MortgageDTO mortgageDTO);
        Task<bool?> UpdateMortgageAsync(Mortgage mortgage);
    }
}
