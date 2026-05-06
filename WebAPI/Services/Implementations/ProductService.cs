using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.DTOs.Products;
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
        public async Task<IEnumerable<ProductBaseDTO>> GetAllAsync()
        {
            var products = await _context.Products
                .ToListAsync();
            var dtos = _mapper.Map<List<ProductBaseDTO>>(products);
            return dtos;
        }

        public async Task<ProductBaseDTO?> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) 
            {
                throw new Exception("Sản phẩm không tồn tại!");
            }
            var dto = _mapper.Map<ProductBaseDTO>(product);
            return dto;
        }
    }
}
