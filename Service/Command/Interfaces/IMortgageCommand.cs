using Domain;
using Domain.DTOs;

namespace Service.Command.Interfaces
{
    public interface IMortgageCommand
    {
        Task<MortgageDTO> CreateMortgageAsync(MortgageDTO mortgageDTO);
        Task<bool?> UpdateMortgageAsync(Mortgage mortgage);
    }
}
