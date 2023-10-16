using Domain.Configuration.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Domain.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public CosmosConfig CosmosConfig { get; set; }
        public BusinessLogicConfig BusinessLogicConfig { get; set; }

        public SendGridConfig SendGridConfig { get; set; }

        public AppConfiguration(IConfiguration configuration)
        {
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
            SendGridConfig = new SendGridConfig
            {
                SendGridApiKey = configuration.GetSection("SendGridConfig:SendGridApiKey").Value ?? "",
                Sender = configuration.GetSection("SendGridConfig:Sender").Value ?? "",
            };
        }
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

    public class SendGridConfig
    {
        public string SendGridApiKey { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
    }
}
