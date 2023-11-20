using System.Diagnostics.CodeAnalysis;

namespace WebshopAPI.Entities
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string StreetName { get; set; }

        [ForeignKey("ZipCode.Id")]
        public int ZipCodeId { get; set; }

        [Column(TypeName ="nvarchar(20)")]
        public string Country { get; set; }

        public ZipCode ZipCode { get; set; }
        public Customer Customer { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();

    }
}
