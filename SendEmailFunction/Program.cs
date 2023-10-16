using DAL;
using DAL.Repository;
using DAL.Repository.Interfaces;
using Domain.Configuration;
using Domain.Configuration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service;
using Service.Interfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services => {
        services.AddSingleton<IAppConfiguration, AppConfiguration>();
        services.AddDbContext<BuyMyHouseContext>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IMortgageService, MortgageService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    })
    .Build();

host.Run();



