namespace DAL.Repository.Interfaces
{
    public interface IAppConfiguration
    {
        MailerConfig MailerConfig { get; }
        CosmosConfig CosmosConfig { get; }
        BusinessLogicConfig BusinessLogicConfig { get; }
    }
}
