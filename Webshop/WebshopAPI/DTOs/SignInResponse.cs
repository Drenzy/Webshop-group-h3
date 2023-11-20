namespace WebshopAPI.DTOs
{
    public class SignInResponse
    {
        public int LoginId { get; set; }
        public Role Role { get; set; }
        public string Token { get; set; }

        public int? CustomerId  { get; set; }
        public int? AddressId { get; set; }
    }
}
