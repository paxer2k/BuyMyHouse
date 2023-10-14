using Domain.Entities;

namespace Domain
{
    public class Customer : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public double AnualIncome { get; set; }
        public string DateOfBirth { get; set; } = string.Empty;
    }
}
