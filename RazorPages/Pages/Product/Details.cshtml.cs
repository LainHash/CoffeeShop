using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.DTOs.Products;
using RazorPages.Services;

namespace RazorPages.Pages.Product
{
    public class DetailsModel : PageModel
    {
        private readonly ProductApiService _api;

        public ProductBaseDTO Product { get; set; } = new ProductBaseDTO();

        public DetailsModel(ProductApiService api)
        {
            _api = api;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var product = await _api.GetByIdAsync(id);

            if (product is null)
                return NotFound(); // hoặc RedirectToPage("/NotFound")

            Product = product;
            return Page();
        }
    }
}
