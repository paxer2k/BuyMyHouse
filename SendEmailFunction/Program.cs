using DAL;
using DAL.Repository;
using DAL.Repository.Interfaces;
using Domain.Configuration;
using Domain.Configuration.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Command;
using Service.Command.Interfaces;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services => {
        services.AddSingleton<IAppConfiguration, AppConfiguration>();
        services.AddDbContext<BuyMyHouseContext>();
        services.AddScoped<IEmailCommandService, EmailCommandService>();
        services.AddScoped<ISendGridMailerCommandService, SendGridMailerCommandService>();
        services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
        services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));

    })
    .Build();

host.Run();



