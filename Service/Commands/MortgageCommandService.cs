﻿using AutoMapper;
using DAL.Repository.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Service.Commands.Interfaces;
using Service.Exceptions;
using System.Globalization;

namespace Service.Commands
{
    public class MortgageCommandService : IMortgageCommandService
    {
        private readonly ICommandRepository<Mortgage> _mortgageCommandRepository;
        private readonly IMapper _mapper;
        public MortgageCommandService(ICommandRepository<Mortgage> mortgageCommandRepository, IMapper mapper)
        {
            _mortgageCommandRepository = mortgageCommandRepository;
            _mapper = mapper;
        }

        public async Task<MortgageDTO> CreateMortgageAsync(MortgageDTO mortgageDTO)
        {
            ValidateMortgageDTO(mortgageDTO);

            Mortgage newMortgage = new Mortgage();

            foreach (var customer in mortgageDTO.Customers)
            {
                ValidateCustomerDTO(customer);

                Customer newCustomer = new Customer
                {
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    DateOfBirth = customer.DateOfBirth,
                    AnualIncome = customer.AnualIncome,
                };

                newMortgage.Customers.Add(newCustomer);
            }

            await _mortgageCommandRepository.CreateAsync(newMortgage);

            return _mapper.Map<MortgageDTO>(newMortgage);
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

            return await _mortgageCommandRepository.UpdateAsync(mortgage);
        }

        /// <summary>
        /// Validation methods
        /// </summary>
        /// <param name="mortgageDTO"></param>
        /// <exception cref="BadRequestException"></exception>
        private void ValidateMortgageDTO(MortgageDTO mortgageDTO)
        {
            if (mortgageDTO == null)
                throw new BadRequestException("The mortage could not be created.");

            if (mortgageDTO.Customers == null || mortgageDTO.Customers.Count <= 0)
                throw new BadRequestException("You are required to fill out the customer(s) details");

            if (mortgageDTO.Customers.Count > 2)
                throw new BadRequestException("You are not allowed to have more than 2 customers for a mortgage.");
        }

        private void ValidateCustomerDTO(CustomerDTO customerDTO)
        {
            if (string.IsNullOrEmpty(customerDTO.FirstName) || string.IsNullOrEmpty(customerDTO.LastName))
                throw new BadRequestException("First name and last name are mandatory to fill out.");

            if (string.IsNullOrEmpty(customerDTO.Email))
                throw new BadRequestException("The email is mandatory to fill out.");

            if (string.IsNullOrEmpty(customerDTO.DateOfBirth))
                throw new BadRequestException("Your date of birth is required to be filled out");

            if (customerDTO.AnualIncome <= 0)
                throw new BadRequestException("Entering you anual income in mandatory.");

            if (!DateTime.TryParseExact(customerDTO.DateOfBirth, "yyyy-MM-dd", null, DateTimeStyles.None, out _))
                throw new ArgumentException("Invalid birthdate format. Please use 'yyyy-MM-dd'.");
        }  
    }
}
