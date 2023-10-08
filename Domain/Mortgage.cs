using Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Mortgage : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public double MortgageAmount { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
