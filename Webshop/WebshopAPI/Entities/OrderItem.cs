namespace WebshopAPI.Entities
{
    public class OrderItem
    {

        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "tinyint")]
        public int Quantity { get; set; }

        [ForeignKey("Order.Id")]
        public int OrderId { get; set; }

        [ForeignKey("Product.Id")]
        public int ProductId { get; set; }

        public Order Order { get; set; }

        public Product Product { get; set; }
    }
}
