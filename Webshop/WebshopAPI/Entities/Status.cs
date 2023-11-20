namespace WebshopAPI.Entities
{
    public class Status
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(32)")]
        public string Name { get; set; }

    }
}
