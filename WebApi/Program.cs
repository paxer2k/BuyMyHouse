using DAL;
using DAL.Repository;
using DAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // easy fix for automapper causing errors
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BuyMyHouseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("MortgageDb")));

// Add repositories for dependency injection
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Add services for dependency injection
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IHouseService, HouseService>();
builder.Services.AddScoped<IMortgageService, MortgageService>();

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
