using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Customer : BaseEntity
    {
        [NotMapped]
        public double MIN_INCOME => 15000; // cannot made const (cannot call in service otherwise)
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public double AnualIncome { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
