namespace Domain.DTOs
{
    public class MortgageResponseDTO
    {
        public List<CustomerDTO> Customers { get; set; } = new List<CustomerDTO>(); // cannot be null create on init

        /// <summary>
        /// Total mortgage amount to be granted (additive if two customers)
        /// </summary>
        public double MortgageAmount { get; set; }

        /// <summary>
        /// Date to track when the mortgage was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Date to track expiry date for the mortgage 
        /// </summary>
        public DateTime ExpiresAt { get; set; }
        public string? ApplicationStatus { get; set; }
    }
}
