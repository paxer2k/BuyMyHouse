using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class BuyMyHouseContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<Mortgage> Mortgages { get; set; }

        public BuyMyHouseContext(DbContextOptions<BuyMyHouseContext> options) : base(options) { }
    }
}
