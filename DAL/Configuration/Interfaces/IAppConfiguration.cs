﻿namespace DAL.Configuration.Interfaces
{
    public interface IAppConfiguration
    {
        MailerConfig MailerConfig { get; }
        CosmosConfig CosmosConfig { get; }
        BusinessLogicConfig BusinessLogicConfig { get; }
    }
}