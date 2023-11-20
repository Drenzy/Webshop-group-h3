namespace WebshopAPI.DTOs
{
    public class LoginRequest
    {
        [Required]
        [StringLength(40, ErrorMessage = "UserName cannot be longer than 40 chars")]
        public string UserName { get; set; } = string.Empty;

        [StringLength(40, ErrorMessage = "Password cannot be longer than 40 chars")]
        public string? Password { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Email cannot be longer than 40 chars")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public Role Role { get; set; }
    }
}
