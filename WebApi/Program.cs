using DAL;
using DAL.Repository;
using DAL.Repository.Interfaces;
using DAL.Seeder;
using DAL.Seeder.Interfaces;
using Domain.Configuration;
using Domain.Configuration.Interfaces;
using Service;
using Service.Interfaces;
using System.Configuration;
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

builder.Services.AddSingleton<ISendGridMailer, SendGridMailer>();

// Add repositories for dependency injection
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
builder.Services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));

// Add services for dependency injection
builder.Services.AddScoped<IMortgageService, MortgageService>();
builder.Services.AddScoped<IMortgageCalculatorService, MortgageCalculatorService>();
builder.Services.AddScoped<IMortgageQuery, MortgageQuery>();
builder.Services.AddScoped<IMortgageCommand, MortgageCommand>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICosmosDataSeeder, CosmosDataSeeder>();

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
