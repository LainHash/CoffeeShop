using RazorPages.DTOs.Products;
using RazorPages.DTOs.Products.Create;
using RazorPages.DTOs.Products.Update;

namespace RazorPages.Services.Interfaces
{
    public interface IProductApiService
    {
        Task<List<ProductDTO>> GetAllAsync();
        Task<ProductDTO?> GetByIdAsync(Guid id);
        Task<bool> CreateAsync(CreateProductDTO dto);
        Task<bool> UpdateAsync(Guid id, UpdateProductDTO dto);
        Task DeleteAsync(Guid id); 
        
    }
}
