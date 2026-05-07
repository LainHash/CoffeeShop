

using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebAPI.DTOs.Products.Create;
using WebAPI.DTOs.Products.Update;
using WebAPI.Helpers.Extensions;
using WebAPI.Services.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);

                return Ok(product);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDTO dto, [FromServices] IValidator<CreateProductDTO> validator)
        {
            var error = await validator.ValidateAndReturnError(dto);
            if (error != null)
            {
                return error;
            }

            var product = await _productService.CreateAsync(dto);

            return Ok(product);

        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateProductDTO dto, [FromServices] IValidator<UpdateProductDTO> validator, Guid id)
        {
            var error = await validator.ValidateAndReturnError(dto);
            if (error != null)
            {
                return error;
            }
            try
            {
                var product = await _productService.UpdateAsync(id, dto);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            

        }
    }
}
