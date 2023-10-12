﻿using AutoMapper;
using DAL.Interfaces;
using DAL.Repository;
using DAL.Repository.Interfaces;
using Domain;
using Domain.DTOs;
using Service.Exceptions;
using Service.Helpers;
using Service.Interfaces;

namespace Service
{
    public class MortgageService : IMortgageService
    {
        private readonly IRepository<Mortgage> _mortgageRepository;
        private readonly IMapper _mapper;
        private readonly IAppConfiguration _appConfiguration;
        public MortgageService(IRepository<Mortgage> mortgageRepository, IMapper mapper, IAppConfiguration appConfiguration)
        {
            _mortgageRepository = mortgageRepository;
            _mapper = mapper;
            _appConfiguration = appConfiguration;
        }

        public async Task<MortgageDTO> CreateMortgageAsync(MortgageDTO mortgageDTO)
        {
            if (mortgageDTO == null)
                throw new BadRequestException("The mortage could not be created.");

            if (mortgageDTO.Customers == null || mortgageDTO.Customers.Count <= 0)
                throw new BadRequestException("You are required to fill out the customer(s) details");

            if (mortgageDTO.Customers.Count > 2)
                throw new BadRequestException("You are not allowed to have more than 2 customers for a mortgage.");

            Mortgage newMortgage = new Mortgage()
            {
                PartitionKey = "1",
                Customers = new List<Customer>()
            };

            foreach (var customer in mortgageDTO.Customers)
            {
                if (MortgageHelper.CalculateAge(customer.DateOfBirth) < _appConfiguration.BusinessLogicConfig.MIN_AGE)
                    throw new BadRequestException("Sorry, but you are not eligeable for a mortage as it is 18+ only.");

                if (customer.AnualIncome < _appConfiguration.BusinessLogicConfig.MIN_INCOME)
                    throw new BadRequestException($"Sorry, you need to earn at least ${_appConfiguration.BusinessLogicConfig.MIN_INCOME} per year in order to sign up for a mortgage");

                Customer newCustomer = new Customer
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    DateOfBirth = customer.DateOfBirth,
                    AnualIncome = customer.AnualIncome,
                    PhoneNumber = customer.PhoneNumber,
                    PartitionKey = "1",
                };

                newMortgage.Customers.Add(newCustomer);
            }

            await _mortgageRepository.CreateAsync(newMortgage);

            return _mapper.Map<MortgageDTO>(newMortgage);
        }

        public async Task<IEnumerable<Mortgage>> GetAllMortgagesAsync()
        {
            var mortgages = await _mortgageRepository.GetAllAsync();

            if (mortgages == null)
                throw new BadRequestException("The mortages could not be retrieved");

            return mortgages;
        }

        public async Task<Mortgage> GetMortgageByIdAsync(Guid id)
        {
            var mortgage = await _mortgageRepository.GetByConditionAsync(m => m.Id == id);

            if (mortgage == null)
                throw new BadRequestException($"The mortgage with id {id} does not exist!");

            if (mortgage.ExpiresAt < DateTime.Now)
                throw new ForbiddenException("The time for viewing this mortgage application has expired!");

            return mortgage;
        }

        /// <summary>
        /// Function responsible for updating any field within the mortgage object in the database (e.g mortgage amount, expiry date, etc..)
        /// </summary>
        /// <param name="mortgage"></param>
        /// <returns></returns>
        /// <exception cref="BadRequestException"></exception>
        public async Task<bool?> UpdateMortgageAsync(Mortgage mortgage)
        {
            if (mortgage == null)
                throw new BadRequestException("The mortgage object was not initialized...");

            if (string.IsNullOrEmpty(mortgage.Id.ToString()))
                throw new BadRequestException("The mortgage does not exist in the database");

            return await _mortgageRepository.UpdateAsync(mortgage);
        }



        public async Task<IEnumerable<Mortgage>> GetAllActiveMortgages()
        {
            return await _mortgageRepository.GetAllByConditionAsync(m => m.CreatedAt == DateTime.Today.AddHours(-24)); // this action will happen on next day
        }
    }
}
