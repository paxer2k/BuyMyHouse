using Domain.Configuration.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Domain.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public SmtpConfig SmtpConfig { get; set; }
        public CosmosConfig CosmosConfig { get; set; }
        public BusinessLogicConfig BusinessLogicConfig { get; set; }

        public AppConfiguration(IConfiguration configuration)
        {
            SmtpConfig = new SmtpConfig
            {
                Sender = configuration.GetSection("SmtpConfig:Sender").Value ?? "",
                Password = configuration.GetSection("SmtpConfig:Password").Value ?? "",
                Server = configuration.GetSection("SmtpConfig:Server").Value ?? "",
                Port = int.Parse(configuration.GetSection("SmtpConfig:Port").Value ?? "587"),
            };
            CosmosConfig = new CosmosConfig
            {
                CosmosUrl = configuration.GetSection("CosmosConfig:CosmosUrl").Value ?? "",
                CosmosPrimaryKey = configuration.GetSection("CosmosConfig:CosmosPrimaryKey").Value ?? "",
                CosmosDbName = configuration.GetSection("CosmosConfig:CosmosDbName").Value ?? ""
            };
            BusinessLogicConfig = new BusinessLogicConfig
            {
                MIN_INCOME = double.Parse(configuration.GetSection("BusinessLogicConfig:MIN_INCOME").Value ?? "15000", CultureInfo.InvariantCulture),
                MIN_AGE = int.Parse(configuration.GetSection("BusinessLogicConfig:MIN_AGE").Value ?? "18"),
                INTEREST_RATE = double.Parse(configuration.GetSection("BusinessLogicConfig:INTEREST_RATE").Value ?? "4.50", CultureInfo.InvariantCulture)
            };
        }
    }

    public class SmtpConfig
    {
        public string Sender { get; internal set; } = string.Empty;
        public string Password { get; internal set; } = string.Empty;
        public string Server { get; internal set; } = string.Empty;
        public int Port { get; internal set; } 
    }

    public class CosmosConfig
    {
        public string CosmosUrl { get; internal set; } = string.Empty;
        public string CosmosPrimaryKey { get; internal set; } = string.Empty;
        public string CosmosDbName { get; internal set; } = string.Empty; 
    }

    public class BusinessLogicConfig
    {
        public double MIN_INCOME { get; internal set; }
        public int MIN_AGE { get; internal set; }
        public double INTEREST_RATE { get; internal set; }
    }
}
