﻿using DAL.Configuration.Interfaces;
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
            modelBuilder.HasDefaultContainer("Mortgages"); // tf am i supposed to do with this

            // Configuring mortgage entity
            modelBuilder.Entity<Mortgage>()
                .ToContainer("Mortgages")
                .HasNoDiscriminator()
                .HasPartitionKey(m => m.PartitionKey)
                .UseETagConcurrency();

            modelBuilder.Entity<Mortgage>()
                .OwnsMany(m => m.Customers, sa =>
                {
                    sa.ToJsonProperty("Customers"); // idk if this is needed
                    sa.Property(c => c.FirstName).ToJsonProperty("FirstName");
                    sa.Property(c => c.LastName).ToJsonProperty("LastName");
                    sa.Property(c => c.Email).ToJsonProperty("Email");
                    sa.Property(c => c.DateOfBirth).ToJsonProperty("DateOfBirth");
                    sa.Property(c => c.AnualIncome).ToJsonProperty("AnualIncome");
                    sa.Property(c => c.PhoneNumber).ToJsonProperty("PhoneNumber");
                });
        }
    }
}
