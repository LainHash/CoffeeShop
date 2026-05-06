using WebAPI.DTOs.Products;

namespace WebAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductBaseDTO>> GetAllAsync();
        Task<ProductBaseDTO?> GetByIdAsync(int id);
    }
}
