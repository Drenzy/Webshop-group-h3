namespace WebshopAPI.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }


        [Column(TypeName = "nvarchar(32)")]
        public string Name { get; set; }

        public List<Product> Products { get; set; }

    }
}
