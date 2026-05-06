using WebAPI.DTOs.Products;
using WebAPI.DTOs.Products.Create;
using WebAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductBaseDTO>> GetAllAsync();
        Task<ProductBaseDTO?> GetByIdAsync(int id);
        Task<ProductBaseDTO> CreateAsync(CreateProductDTO dto);
    }
}
