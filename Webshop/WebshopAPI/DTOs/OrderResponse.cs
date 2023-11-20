namespace WebshopAPI.DTOs
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AddressId { get; set; }
        public DateTime OrderDate { get; set; }
        public int StatusId { get; set; }

        public OrderCustomerResponse Customer { get; set; }
        public OrderAddressResponse Address { get; set; }
        public OrderStatusResponse Status { get; set; }
        public List<OrderOrderItemResponse> OrderItems { get; set; } = new List<OrderOrderItemResponse>();

    }
    public class OrderCustomerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNr { get; set; }
        public string Email { get; set; }
    }
    public class OrderStatusResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class OrderOrderItemResponse
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string? Name { get; set; }
    }

    public class OrderAddressResponse
    {
        public int Id { get; set; }
        public string StreetName { get; set; }
        public int ZipCodeId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }

}
