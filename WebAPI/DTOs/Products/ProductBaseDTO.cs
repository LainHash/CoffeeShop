namespace WebAPI.DTOs.Products
{
    public class ProductBaseDTO
    {
        public Guid PublicId { get; set; } = new Guid();

        public string ProductName { get; set; } = null!;

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; }
    }
}
