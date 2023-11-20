namespace WebshopAPI.Entities
{
    public class ZipCode
 
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string City { get; set; }
    }
}
