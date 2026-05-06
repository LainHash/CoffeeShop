namespace RazorPages.DTOs.Products
{
    public class ProductBaseDTO
    {
        public Guid PublicId { get; set; }

        public string ProductName { get; set; } = null!;

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }

        public bool IsAvailable { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
