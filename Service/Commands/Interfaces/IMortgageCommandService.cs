using Domain;
using Domain.DTOs;

namespace Service.Commands.Interfaces
{
    public interface IMortgageCommandService
    {
        Task<MortgageDTO> CreateMortgageAsync(MortgageDTO mortgageDTO);
        Task<bool?> UpdateMortgageAsync(Mortgage mortgage);
    }
}
