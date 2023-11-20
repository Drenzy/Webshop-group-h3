namespace WebshopAPI.DTOs
{
    public class CustomerRegisterRequest
    {
        [Required]
        [StringLength(40, ErrorMessage = "Username cannot be longer than 40 chars")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(40, ErrorMessage = "Password cannot be longer than 40 chars")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(40, ErrorMessage = "Email cannot be longer than 40 chars")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(40, ErrorMessage = "Username cannot be longer than 40 chars")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(8, ErrorMessage = "PhoneNumber cannot be longer than 8 chars")]
        public string PhoneNr { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "Title cannot be longer than 50 chars")]
        public string StreetName { get; set; } = string.Empty;

        [Required]
        public int ZipCode { get; set; } = 0;

        [Required]
        [StringLength(20, ErrorMessage = "Title cannot be longer than 20 chars")]
        public string Country { get; set; } = string.Empty;
    }
}
