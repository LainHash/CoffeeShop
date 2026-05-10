using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.DTOs.Customers;
using RazorPages.Services.Interfaces;

namespace RazorPages.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly ICustomerApiService _customerService;

        public RegisterModel(ICustomerApiService customerService)
        {
            _customerService = customerService;
        }

        [BindProperty]
        public RegisterRequestDTO Input { get; set; } = new();

        public string? ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var (success, message) = await _customerService.RegisterAsync(Input);

            if (!success)
            {
                ErrorMessage = message;
                return Page();
            }

            // Sau khi đăng ký thành công, chuyển về Login với thông báo
            return RedirectToPage("/Account/Login",
                new { message = "Đăng ký thành công! Vui lòng kiểm tra email để xác nhận tài khoản." });
        }
    }
}
