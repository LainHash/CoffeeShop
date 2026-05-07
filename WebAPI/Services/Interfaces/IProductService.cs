
using Microsoft.AspNetCore.Http.HttpResults;
using WebAPI.DTOs.Products;
using WebAPI.DTOs.Products.Create;
using WebAPI.DTOs.Products.Update;

namespace WebAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<ProductDTO?> GetByIdAsync(Guid id);
        Task<ProductDTO> CreateAsync(CreateProductDTO dto);
        Task<ProductDTO> UpdateAsync(Guid id, UpdateProductDTO dto);
        Task DeleteAsync(Guid id);
    }
}
