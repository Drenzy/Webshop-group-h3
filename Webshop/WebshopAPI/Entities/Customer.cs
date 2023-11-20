namespace WebshopAPI.Entities
{
    public class Customer
    {
        [Key] 
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(40)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(8)")]
        public string PhoneNr { get; set; }

        [ForeignKey("Address.Id")]
        public int AddressId { get; set; }

        [ForeignKey("Login.Id")]
        public int LoginId { get; set; }

        public Login Login { get; set; }
        public Address Address { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
