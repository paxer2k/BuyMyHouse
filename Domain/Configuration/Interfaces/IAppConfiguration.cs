namespace Domain.Configuration.Interfaces
{
    public interface IAppConfiguration
    {
        CosmosConfig CosmosConfig { get; }
        BusinessLogicConfig BusinessLogicConfig { get; }
        SendGridConfig SendGridConfig { get; }
    }
}
