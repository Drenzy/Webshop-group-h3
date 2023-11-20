namespace WebshopAPI.DTOs
{
    public class ProductRequest
    {
        [Required]
        [StringLength(32, ErrorMessage = "Title cannot be longer than 32 chars")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; } = 0;

        [Required]
        [StringLength(32, ErrorMessage = "Title cannot be longer than 255 chars")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }
    }
}
