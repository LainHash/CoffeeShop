using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.DTOs.Products;
using WebAPI.DTOs.Products.Create;
using WebAPI.DTOs.Products.Update;
using WebAPI.Helpers.Extensions;
using WebAPI.Models;
using WebAPI.Services.Interfaces.Products;

namespace WebAPI.Services.Implementations.Products
{
    public class ProductService : IProductService
    {
        private readonly CoffeeShopDbContext _context;
        private readonly IMapper _mapper;
        public ProductService(CoffeeShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var query = _context.Products;

            var products = await query
                .Where(p => p.IsAvailable)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ProductDTO>>(products);

            return dtos;
        }

        public async Task<ProductDTO?> GetByIdAsync(Guid id)
        {
            var query = _context.Products;

            var product = await query
                .FirstOrDefaultAsync(p => p.PublicId == id);
            if(product == null)
            {
                throw new Exception("Sản phẩm không tồn tại!");
            }
            if (!product.IsAvailable) 
            {
                throw new Exception("Sản phẩm đã bị xóa!");
            }

            var dto = _mapper.Map<ProductDTO>(product);

            return dto;
        }

        public async Task<ProductDTO> CreateAsync(CreateProductDTO dto)
        {

            var product = _mapper.Map<Product>(dto);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> UpdateAsync(Guid id, UpdateProductDTO dto)
        {
            var query = _context.Products;

            var product = await query
                .FirstOrDefaultAsync(p => p.PublicId == id);
            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại!");
            }

            _mapper.Map(dto, product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            var query = _context.Products;
            var product = await query
                .FirstOrDefaultAsync(p => p.PublicId == id);

            if (product == null)
            {
                throw new KeyNotFoundException("Sản phẩm không tồn tại!");
            }

            product.IsAvailable = false;

            await _context.SaveChangesAsync();

        }
    }
}
