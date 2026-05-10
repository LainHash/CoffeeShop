using RazorPages.DTOs.Customers;

namespace RazorPages.Services.Interfaces
{
    public interface ICustomerApiService
    {
        /// <summary>Gọi POST /api/Customers/login. Trả về (thành công, thông báo/lỗi).</summary>
        Task<(bool Success, string Message)> LoginAsync(LoginRequestDTO dto);

        /// <summary>Gọi POST /api/Customers/register. Trả về (thành công, thông báo/lỗi).</summary>
        Task<(bool Success, string Message)> RegisterAsync(RegisterRequestDTO dto);
    }
}
