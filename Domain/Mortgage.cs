using Domain.Entities;

namespace Domain
{
    public class Mortgage : BaseEntity
    {
        public IList<Customer> Customers { get; set; }
        public double MortgageAmount { get; set; }
        public DateTime CreatedAt => DateTime.Now;
        public DateTime ExpiresAt { get; set; }
    }
}
