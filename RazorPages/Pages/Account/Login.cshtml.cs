using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.DTOs.Customers;
using RazorPages.Services.Interfaces;

namespace RazorPages.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly ICustomerApiService _customerService;

        public LoginModel(ICustomerApiService customerService)
        {
            _customerService = customerService;
        }

        [BindProperty]
        public LoginRequestDTO Input { get; set; } = new();

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        public IActionResult OnGet(string? message)
        {
            
            if (!string.IsNullOrEmpty(message))
                SuccessMessage = message;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl)
        {
            if (!ModelState.IsValid)
                return Page();

            var (success, message) = await _customerService.LoginAsync(Input);

            if (!success)
            {
                ErrorMessage = message;
                return Page();
            }

            
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToPage("/Index");
        }
    }
}
