namespace WebshopAPI.DTOs
{
    public class OrderItemResponse
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
    }
}
