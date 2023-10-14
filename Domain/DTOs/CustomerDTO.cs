namespace Domain.DTOs
{
    public class CustomerDTO
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public double AnualIncome { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
