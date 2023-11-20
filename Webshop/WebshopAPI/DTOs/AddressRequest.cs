namespace WebshopAPI.DTOs
{
    public class AddressRequest
    {
        [Required]
        [StringLength(50, ErrorMessage = "Title cannot be longer than 50 chars")]
        public string StreetName { get; set; } = string.Empty;

        [Required]
        public int ZipCodeId { get; set; } = 0;

        [Required]
        [StringLength(20, ErrorMessage = "Title cannot be longer than 20 chars")]
        public string Country { get; set; } = string.Empty;
    }
}
