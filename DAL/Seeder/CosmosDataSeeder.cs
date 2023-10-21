using DAL.Repository.Interfaces;
using DAL.Seeder.Interfaces;
using Domain;

namespace DAL.Seeder
{
    public class CosmosDataSeeder : ICosmosDataSeeder
    {
        private readonly IQueryRepository<Mortgage> _mortgageQueryRepository;
        private readonly ICommandRepository<Mortgage> _mortgageCommandRepository;
        public CosmosDataSeeder(IQueryRepository<Mortgage> mortgageQueryRepository, ICommandRepository<Mortgage> mortgageCommandRepository)
        {
            _mortgageQueryRepository = mortgageQueryRepository;
            _mortgageCommandRepository = mortgageCommandRepository; 
        }

        public async Task SeedDataAsync()
        {
            if (!(await _mortgageQueryRepository.GetAllAsync()).Any())
            {
                var customer1 = new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@example.com",
                    AnualIncome = 75000,
                    DateOfBirth = "1985-3-15"
                };

                var customer2 = new Customer
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane@example.com",
                    AnualIncome = 80000,
                    DateOfBirth = "1988-7-20"/*.ToString("yyyy-MM-dd")*/
                };

                var customer3 = new Customer
                {
                    FirstName = "Alice",
                    LastName = "Johnson",
                    Email = "alice@example.com",
                    AnualIncome = 90000,
                    DateOfBirth = "1990-5-10"/*.ToString("yyyy-MM-dd")*/
                };

                var customer4 = new Customer
                {
                    FirstName = "Alex",
                    LastName = "Arkhipov",
                    Email = "alex.arkhipov867@gmail.com",
                    AnualIncome = 85000,
                    DateOfBirth = "1987-9-25"/*.ToString("yyyy-MM-dd")*/
                };

                var mortgage1 = new Mortgage();
                mortgage1.Customers.Add(customer1);
                mortgage1.Customers.Add(customer2);

                var mortgage2 = new Mortgage();
                mortgage2.Customers.Add(customer3);

                var mortgage3 = new Mortgage();
                mortgage3.Customers.Add(customer4);

                List<Mortgage> mortgages = new List<Mortgage> {mortgage1, mortgage2, mortgage3};

                foreach(var mortgage in mortgages)
                {
                    await _mortgageCommandRepository.CreateAsync(mortgage);
                }
            }
        }
    }
}
