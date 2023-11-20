namespace WebshopAPI.Entities
{
    public class Product 
    { 
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string Name { get; set; }

        [Column(TypeName ="decimal(6,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "nvarchar(255)")]
        public string Description { get; set; }

        [ForeignKey("Category.Id")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
