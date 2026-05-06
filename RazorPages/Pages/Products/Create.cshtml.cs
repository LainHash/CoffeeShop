using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPages.DTOs.Products.Create;
using RazorPages.Services.Interfaces;
using RazorPages.Services.Products;

namespace RazorPages.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly IProductApiService _productApiService;
        private readonly ICategoryApiService _categoryApiService;

        public CreateModel(
            IProductApiService productApiService,
            ICategoryApiService categoryApiService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
        }

        [BindProperty]
        public CreateProductDTO Product { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = [];

        public async Task OnGetAsync()
        {
            await LoadCategoriesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return Page();
            }

            var ok = await _productApiService.CreateAsync(Product);

            if (!ok)
            {
                ModelState.AddModelError(string.Empty, "Tạo sản phẩm thất bại");
                await LoadCategoriesAsync();
                return Page();
            }

            return RedirectToPage("Index");
        }

        private async Task LoadCategoriesAsync()
        {
            var categories = await _categoryApiService.GetAllAsync();

            Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();
        }
    }
}
