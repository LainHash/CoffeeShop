namespace RazorPages.DTOs.Products.Update
{
    public class UpdateProductDTO
    {
        public string? ProductName { get; set; }

        public int? CategoryId { get; set; }

        public decimal? Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }
    }
}
