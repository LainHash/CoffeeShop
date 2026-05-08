namespace WebAPI.DTOs.Accounts.Customers
{
    public class CustomerDTO
    {
        public Guid PublicId { get; set; }

        public string FullName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Username { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        
    }
}
