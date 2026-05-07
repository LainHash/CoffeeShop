using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.DTOs.Products;
using RazorPages.Services.Implementations;
using RazorPages.Services.Interfaces;

namespace RazorPages.Pages.Products
{
    public class DeleteModel : PageModel
    {
        private readonly IProductApiService _productService;

        public DeleteModel(IProductApiService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Guid Id { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Id = id;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _productService.DeleteAsync(Id);

                TempData["Success"] = "Xóa thành công";

                return RedirectToPage("/Products/Index");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                return Page();
            }
        }
    }
}
