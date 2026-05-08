namespace WebAPI.DTOs.Accounts.Customers
{
    public class LoginDTO
    {
        public string Phone { get; set; } = null!;

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
