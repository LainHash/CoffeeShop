using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPages.DTOs.Products.Update;
using RazorPages.Services.Interfaces;

namespace RazorPages.Pages.Products
{
    public class EditModel : PageModel
    {
        private readonly IProductApiService _productApiService;
        private readonly ICategoryApiService _categoryApiService;

        public EditModel(
            IProductApiService productApiService,
            ICategoryApiService categoryApiService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
        }

        [BindProperty]
        public UpdateProductDTO Product { get; set; } = new();

        public List<SelectListItem> Categories { get; set; } = [];

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var item = await _productApiService.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            Product = new UpdateProductDTO
            {
                ProductName = item.ProductName,
                CategoryId = item.CategoryId,
                Price = item.Price,
                ImageUrl = item.ImageUrl,
                Description = item.Description
            };

            await LoadCategoriesAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoriesAsync();
                return Page();
            }

            var ok = await _productApiService.UpdateAsync(id, Product);

            if (!ok)
            {
                ModelState.AddModelError(string.Empty, "Cập nhật sản phẩm thất bại");
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
