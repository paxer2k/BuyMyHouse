﻿using DAL.Seeder.Interfaces;
using Domain;

namespace DAL.Seeder
{
    public class CosmosDataSeeder : ICosmosDataSeeder
    {
        private readonly BuyMyHouseContext _context;
        public CosmosDataSeeder(BuyMyHouseContext context)
        {
            _context = context; 
        }

        public void SeedData()
        {
            if (!_context.Mortgages.AsEnumerable().Any())
            {
                var customer1 = new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com",
                    AnualIncome = 75000,
                    PhoneNumber = "123-456-7890",
                    DateOfBirth = new DateOnly(1985, 3, 15).ToString("yyyy-MM-dd")
                };

                var customer2 = new Customer
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane@example.com",
                    AnualIncome = 80000,
                    PhoneNumber = "987-654-3210",
                    DateOfBirth = new DateOnly(1988, 7, 20).ToString("yyyy-MM-dd")
                };

                var customer3 = new Customer
                {
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice@example.com",
                    AnualIncome = 90000,
                    PhoneNumber = "111-222-3333",
                    DateOfBirth = new DateOnly(1990, 5, 10).ToString("yyyy-MM-dd")
                };

                var customer4 = new Customer
                {
                    FirstName = "Alex",
                    LastName = "Arkhipov",
                    Email = "alex.arkhipov.7590@gmail.com",
                    AnualIncome = 85000,
                    PhoneNumber = "555-666-7777",
                    DateOfBirth = new DateOnly(1987, 9, 25).ToString("yyyy-MM-dd")
                };

                var mortgage1 = new Mortgage
                {
                    Customers = new List<Customer> { customer1, customer2 },
                    CreatedAt = DateTime.Now
                };

                var mortgage2 = new Mortgage
                {
                    Customers = new List<Customer> { customer3 },
                    CreatedAt = DateTime.Now
                };

                var mortgage3 = new Mortgage
                {
                    Customers = new List<Customer> { customer4 },
                    CreatedAt = DateTime.Now
                };

                _context.Mortgages.Add(mortgage1);
                _context.Mortgages.Add(mortgage2);
                _context.Mortgages.Add(mortgage3);

                _context.SaveChanges();
            }
        }
    }
}
