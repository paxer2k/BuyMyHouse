using DAL.Configuration.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace DAL.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public MailerConfig MailerConfig { get; set; }
        public CosmosConfig CosmosConfig { get; set; }
        public BusinessLogicConfig BusinessLogicConfig { get; set; }

        public AppConfiguration(IConfiguration configuration)
        {
            MailerConfig = new MailerConfig
            {
                GridApiKey = configuration.GetSection("MailerConfig:GridApiKey").Value ?? "",
                MailSender = configuration.GetSection("MailerConfig:MailSender").Value ?? ""
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

    public class MailerConfig
    {
        public string? GridApiKey { get; internal set; }
        public string? MailSender { get; internal set; }
    }

    public class CosmosConfig
    {
        public string? CosmosUrl { get; internal set; }
        public string? CosmosPrimaryKey { get; internal set; }
        public string? CosmosDbName { get; internal set; }
    }

    public class BusinessLogicConfig
    {
        public double MIN_INCOME { get; internal set; }
        public int MIN_AGE { get; internal set; }
        public double INTEREST_RATE { get; internal set; }
    }
}
