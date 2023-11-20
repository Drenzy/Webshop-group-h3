namespace WebshopAPI.DTOs
{
    public class CategoryRequest
    {
        [Required]
        [StringLength(32, ErrorMessage = "Name cannot be longer than 32 chars")]
        public string Name { get; set; }
    }
}
