﻿using Domain;
using Domain.DTOs;

namespace Service.Interfaces
{
    public interface IMortgageService
    {
        Task<Mortgage> CreateMortgageAsync(MortgageDTO mortgageDTO);
        Task<IEnumerable<Mortgage>> GetAllMortgagesAsync();
        Task<Mortgage> GetMortgageByIdAsync(Guid id);
        Task<Mortgage> GetMortgageByCustomerIdAsync(Guid customerId);
        Task<bool> HasSentApplication(Guid customerId);
        Task CalculateMortgage();
    }
}
