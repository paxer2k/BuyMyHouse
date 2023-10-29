using Domain.DTOs;
using Domain.Entities;

namespace Service.Commands.Interfaces
{
    public interface IMortgageCommandService
    {
        Task<MortgageDTO> CreateMortgageAsync(MortgageDTO mortgageDTO);
        Task<bool?> UpdateMortgageAsync(Mortgage mortgage);
    }
}
