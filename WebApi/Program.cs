using DAL;
using DAL.Repository;
using DAL.Repository.Interfaces;
using DAL.Seeder;
using DAL.Seeder.Interfaces;
using Domain.Configuration;
using Domain.Configuration.Interfaces;
using Service.Commands;
using Service.Commands.Interfaces;
using Service.Queries;
using Service.Queries.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 
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
builder.Services.AddSingleton<ISendGridMailerCommandService, SendGridMailerCommandService>();

// Add repositories for dependency injection
builder.Services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
builder.Services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
builder.Services.AddScoped<ICosmosDataSeeder, CosmosDataSeeder>();

// Add services
builder.Services.AddScoped<IMortgageQueryService, MortgageQueryService>();
builder.Services.AddScoped<IMortgageCommandService, MortgageCommandService>();
builder.Services.AddScoped<ICalculateMortgageCommandService, CalculateMortgageCommandService>();
builder.Services.AddScoped<IEmailCommandService, EmailCommandService>();

var app = builder.Build();

// seed if no data exists... and ensure creation
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BuyMyHouseContext>();
    context.Database.EnsureCreated();

    var dataSeeder = scope.ServiceProvider.GetRequiredService<ICosmosDataSeeder>();

    await dataSeeder.SeedDataAsync();
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
