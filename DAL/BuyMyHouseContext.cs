using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class BuyMyHouseContext : DbContext
    {
        private readonly AppConfiguration _appConfiguration;

        public DbSet<Customer> Customers { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Mortgage> Mortgages { get; set; }

        public BuyMyHouseContext(AppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseCosmos(
                accountEndpoint: _appConfiguration.CosmosUrl,
                accountKey: _appConfiguration.CosmosPrimaryKey,
                databaseName: _appConfiguration.CosmosDbName);


/*        public BuyMyHouseContext(DbContextOptions<BuyMyHouseContext> options) : base(options) { }
*/    }
}
