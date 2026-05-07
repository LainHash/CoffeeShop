using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.DTOs.Products;
using RazorPages.Services.Interfaces;

namespace RazorPages.Pages.Product
{
    public class DetailsModel : PageModel
    {
        private readonly IProductApiService _productService;

        public ProductDTO Product { get; set; } = new ProductDTO();

        public DetailsModel(IProductApiService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product is null)
                return NotFound(); // hoặc RedirectToPage("/NotFound")

            Product = product;
            return Page();
        }
    }
}
