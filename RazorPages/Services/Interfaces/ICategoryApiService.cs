using RazorPages.DTOs.Categories;

namespace RazorPages.Services.Interfaces
{
    public interface ICategoryApiService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO?> GetByIdAsync(int id);
    }
}
