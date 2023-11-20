namespace WebshopAPI.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer.Id")]
        public int CustomerId { get; set; }

        [ForeignKey("Address.Id")]
        public int AddressId { get; set; }

        public DateTime OrderDate { get; set; }

        [ForeignKey("Status.Id")]
        public int StatusId { get; set; }

        public Customer Customer { get; set; }
        public Status Status { get; set; }
        public List<OrderItem> OrderItems { get; set; } 
        public Address Address { get; set; }
    }
}
