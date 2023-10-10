using Newtonsoft.Json;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "/mortgageId")]
        public string? PartitionKey { get; set; }
    }
}
