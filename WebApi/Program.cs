using DAL;
using DAL.Configuration;
using DAL.Configuration.Interfaces;
using DAL.Repository;
using DAL.Repository.Interfaces;
using DAL.Seeder;
using DAL.Seeder.Interfaces;
using Service;
using Service.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // easy fix for automapper causing errors
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Environment.CurrentDirectory)
    .AddJsonFile("appsettings.json", true, true)
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddDbContext<BuyMyHouseContext>(); //(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MortgageDb")));

builder.Services.AddSingleton<IAppConfiguration>(new AppConfiguration(configuration));

// Add repositories for dependency injection
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Add services for dependency injection
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IHouseService, HouseService>();
builder.Services.AddScoped<IMortgageService, MortgageService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICosmosDataSeeder, CosmosDataSeeder>();

var app = builder.Build();

// seed if no data exists...
using (var scope = app.Services.CreateScope())
{
    var dataSeeder = scope.ServiceProvider.GetRequiredService<ICosmosDataSeeder>();

    dataSeeder.SeedData();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
