
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

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CoffeeShopDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(CoffeeShopDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.Products;
            var products = await query.ToListAsync();
            var dtos = _mapper.Map<List<ProductDTO>>(products);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = _context.Products;

            var product = await query
                .FirstOrDefaultAsync(p => p.PublicId == id);
            if (product == null)
            {
                return BadRequest("Sản phẩm không tồn tại!");
            }

            var dtos = _mapper.Map<ProductDTO>(product);

            return Ok(dtos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO dto, [FromServices] IValidator<CreateProductDTO> validator)
        {
            var error = await validator.ValidateAndReturnError(dto);
            if (error != null)
            {
                return error;
            }

            var product = _mapper.Map<Product>(dto);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProductDTO>(product));

        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateProductDTO dto, [FromServices] IValidator<UpdateProductDTO> validator, Guid id)
        {
            var error = await validator.ValidateAndReturnError(dto);
            if (error != null)
            {
                return error;
            }

            var query = _context.Products;

            var product = await query
                .FirstOrDefaultAsync(p => p.PublicId == id);
            if (product == null)
            {
                return BadRequest("Sản phẩm không tồn tại!");
            }

            _mapper.Map(dto, product);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProductDTO>(product));
        }
    }
}
