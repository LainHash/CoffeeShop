using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.DTOs.Products;
using RazorPages.Services;

namespace RazorPages.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly ProductApiService _api;
        public List<ProductBaseDTO> Products { get; set; } = [];

        public IndexModel(ProductApiService api) => _api = api;

        public async Task OnGetAsync()
        {
            Products = await _api.GetAllAsync();
        }
    }
}
