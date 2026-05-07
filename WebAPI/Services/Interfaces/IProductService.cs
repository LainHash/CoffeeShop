using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs.Products;
using WebAPI.DTOs.Products.Create;
using WebAPI.DTOs.Products.Update;

namespace WebAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<ProductDTO?> GetByIdAsync(Guid id);
        Task<CreateProductDTO> CreateAsync(CreateProductDTO dto);
        Task<UpdateProductDTO> UpdateAsync(Guid id, UpdateProductDTO dto);
    }
}
