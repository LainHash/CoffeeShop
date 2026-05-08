using WebAPI.DTOs.Accounts.Customers;

namespace WebAPI.Services.Interfaces.Auths
{
    public interface ICustomerService
    {
        Task<string> LoginAsync(LoginDTO dto);
    }
}
