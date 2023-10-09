using DAL;
using DAL.Repository;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddSingleton(new AppConfiguration(configuration));

// Add repositories for dependency injection
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Add services for dependency injection
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IHouseService, HouseService>();
builder.Services.AddScoped<IMortgageService, MortgageService>();
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

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
