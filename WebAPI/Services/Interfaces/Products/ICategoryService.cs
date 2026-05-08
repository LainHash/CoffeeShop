using WebAPI.DTOs.Categories;
using WebAPI.DTOs.Products;

namespace WebAPI.Services.Interfaces.Products
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO?> GetByIdAsync(int id);
    }
}
