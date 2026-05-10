using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPages.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // Xóa session JWT và Username
            HttpContext.Session.Remove("JwtToken");
            HttpContext.Session.Remove("Username");

            return RedirectToPage("/Index");
        }
    }
}
