using DAL.Seeder.Interfaces;
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
            if (!_context.Mortgages.Any())
            {
                var customer1 = new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com",
                    AnualIncome = 75000,
                    PhoneNumber = "123-456-7890",
                    DateOfBirth = new DateOnly(1985, 3, 15)
                };

                var customer2 = new Customer
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane@example.com",
                    AnualIncome = 80000,
                    PhoneNumber = "987-654-3210",
                    DateOfBirth = new DateOnly(1988, 7, 20)
                };

                var customer3 = new Customer
                {
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice@example.com",
                    AnualIncome = 90000,
                    PhoneNumber = "111-222-3333",
                    DateOfBirth = new DateOnly(1990, 5, 10)
                };

                var customer4 = new Customer
                {
                    FirstName = "Alex",
                    LastName = "Arkhipov",
                    Email = "647833@student.inholland.nl",
                    AnualIncome = 85000,
                    PhoneNumber = "555-666-7777",
                    DateOfBirth = new DateOnly(1987, 9, 25)
                };

                var mortgage1 = new Mortgage
                {
                    Customers = new List<Customer> { customer1, customer2 }
                };

                var mortgage2 = new Mortgage
                {
                    Customers = new List<Customer> { customer3 }
                };

                var mortgage3 = new Mortgage
                {
                    Customers = new List<Customer> { customer4 }
                };

                _context.Mortgages.Add(mortgage1);
                _context.Mortgages.Add(mortgage2);
                _context.Mortgages.Add(mortgage3);

                _context.SaveChanges();
            }
        }
    }
}
