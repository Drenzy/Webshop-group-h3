namespace WebshopAPI.DTOs
{
    public class OrderItemRequest
    {

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }
    }
}
