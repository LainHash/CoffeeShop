

namespace WebAPI.DTOs.Products.Create
{
    public class CreateProductDTO
    {
        public string ProductName { get; set; } = null!;

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }
    }
}
