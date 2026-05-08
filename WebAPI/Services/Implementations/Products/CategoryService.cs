using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.DTOs.Categories;
using WebAPI.Services.Interfaces.Products;

namespace WebAPI.Services.Implementations.Products
{
    public class CategoryService : ICategoryService
    {
        private readonly CoffeeShopDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(CoffeeShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
        {
            var query = _context.Categories;
            var categories = await query
                .ToListAsync();
            var dtos = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
            return dtos;
        }

        public async Task<CategoryDTO?> GetByIdAsync(int id)
        {
            var query = _context.Categories;
            var category = await query
                .FirstOrDefaultAsync(c => c.CategoryId == id);
            var dto = _mapper.Map<CategoryDTO>(category);
            return dto;
        }
    }
}
