namespace Domain.DTOs
{
    public class MortgageDTO
    {
        public Guid CustomerId { get; set; }
        public double MortgageAmount { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
