using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.DTOs.Products;
using WebAPI.DTOs.Products.Create;
using WebAPI.DTOs.Products.Update;
using WebAPI.Helpers.Extensions;
using WebAPI.Models;
using WebAPI.Services.Interfaces;

namespace WebAPI.Services.Implementations
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

            var dto = _mapper.Map<ProductDTO>(product);

            return dto;
        }

        public async Task<CreateProductDTO> CreateAsync(CreateProductDTO dto)
        {

            var product = _mapper.Map<Product>(dto);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<CreateProductDTO>(product);
        }

        public async Task<UpdateProductDTO> UpdateAsync(Guid id, UpdateProductDTO dto)
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

            return _mapper.Map<UpdateProductDTO>(product);
        }
    }
}
