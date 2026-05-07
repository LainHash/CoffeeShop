using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.DTOs.Products;

using RazorPages.Services.Interfaces;

namespace RazorPages.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly IProductApiService _productService;
        public List<ProductDTO> Products { get; set; } = [];

        public IndexModel(IProductApiService productService)
        {
            _productService = productService;
        }

        public async Task OnGetAsync()
        {
            Products = await _productService.GetAllAsync();
        }
    }
}
