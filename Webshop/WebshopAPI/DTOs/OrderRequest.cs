namespace WebshopAPI.DTOs
{
    public class OrderRequest
    {

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int AddressId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public int StatusId { get; set; }
    }
}
