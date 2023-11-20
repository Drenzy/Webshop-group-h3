namespace WebshopAPI.DTOs
{
    public class OrderCreateRequest
    {
        [Required]
        public int CustomerId { get; set; }

        public int AddressId { get; set; } 

        [Required]
        public List<OrderCreateRequestOrderItem> OrderItems { get; set;} =  new List<OrderCreateRequestOrderItem>();

        public OrderCreateAddressRequest Address { get; set; }
    }
    public class OrderCreateRequestOrderItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderCreateAddressRequest
    {
        [StringLength(50, ErrorMessage = "Title cannot be longer than 50 chars")]
        public string StreetName { get; set; } = string.Empty;
        public int ZipCodeId { get; set; } = 0;

        [StringLength(20, ErrorMessage = "Title cannot be longer than 20 chars")]
        public string Country { get; set; } = string.Empty;
    }
}
