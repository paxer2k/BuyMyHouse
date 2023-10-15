namespace Domain.Configuration.Interfaces
{
    public interface IAppConfiguration
    {
        SmtpConfig SmtpConfig { get; }
        CosmosConfig CosmosConfig { get; }
        BusinessLogicConfig BusinessLogicConfig { get; }
    }
}
