using Newtonsoft.Json;

namespace Domain.DTOs
{
    public class CustomerDTO
    {
        [JsonRequired]
        public string? FirstName { get; set; }

        [JsonRequired]
        public string? LastName { get; set; }

        [JsonRequired]
        public string? Email { get; set; }

        [JsonRequired]
        public double AnualIncome { get; set; }

        public string? PhoneNumber { get; set; }

        [JsonRequired]
        public DateOnly DateOfBirth { get; set; }
    }
}
