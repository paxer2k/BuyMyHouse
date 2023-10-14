using DAL;
using DAL.Configuration;
using DAL.Configuration.Interfaces;
using DAL.Repository;
using DAL.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service;
using Service.Interfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services => {
        services.AddSingleton<IAppConfiguration, AppConfiguration>();
        services.AddDbContext<BuyMyHouseContext>();
        services.AddScoped<IMortgageCalculatorService, MortgageCalculatorService>();
        services.AddScoped<IMortgageService, MortgageService>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    })
    .Build();

host.Run();



