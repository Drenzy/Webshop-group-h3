namespace WebshopAPI.DTOs
{
    public class CustomerRequest
    {
        [Required]
        [StringLength(40, ErrorMessage = "Username cannot be longer than 40 chars")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public int AddressId { get; set; }

        [Required]
        [StringLength(8, ErrorMessage = "PhoneNumber cannot be longer than 8 chars")]
        public string PhoneNr { get; set; } = string.Empty;

        [Required]
        public int LoginId { get; set; }
    }
}
