using DAL.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class BuyMyHouseContext : DbContext
    {
        private readonly IAppConfiguration _appConfiguration;

/*        public DbSet<Customer> Customers { get; set; }
        public DbSet<House> Houses { get; set; }*/
        public DbSet<Mortgage> Mortgages { get; set; }

        public BuyMyHouseContext(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseCosmos(
                accountEndpoint: _appConfiguration.CosmosConfig.CosmosUrl!,
                accountKey: _appConfiguration.CosmosConfig.CosmosPrimaryKey!,
                databaseName: _appConfiguration.CosmosConfig.CosmosDbName!);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("Mortgages");

            modelBuilder.Entity<Mortgage>()
                .ToContainer("Mortgages");

            modelBuilder.Entity<Mortgage>()
                .HasNoDiscriminator();

            modelBuilder.Entity<Mortgage>()
                .HasPartitionKey(m => m.PartitionKey);

            modelBuilder.Entity<Mortgage>()
                .UseETagConcurrency();

/*            modelBuilder.Entity<Mortgage>().OwnsMany(
                m => m.Customers,
                sa =>
                {
                    sa.ToJsonProperty("Customers");
                    sa.Property(p => p.Street).ToJsonProperty("ShipsToStreet");
                    sa.Property(p => p.City).ToJsonProperty("ShipsToCity");
                });*/
        }

        /*        public BuyMyHouseContext(DbContextOptions<BuyMyHouseContext> options) : base(options) { }
        */
    }
}
