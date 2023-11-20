using WebshopAPI.Authentication;

namespace WebshopAPI.Entities
{
    public class Login
    {
        [Key] 
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(40)")]
        public string UserName { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Password { get; set; }

        [Column(TypeName = "nvarchar(40)")]
        public string Email { get; set; }

        public Role Role { get; set; }

        public Customer Customer { get; set; }
    }
}
