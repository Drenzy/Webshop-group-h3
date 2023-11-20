namespace WebshopAPI.DTOs
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public string Token { get; set; }

        public LoginCustomerResponse Customer { get; set; }
        public LoginAddressResponse Address { get; set; }
    }

    public class LoginAddressResponse
    {
        public int Id { get; set; }
        public string StreetName { get; set; }
        public int ZipCodeId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }

    public class LoginCustomerResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AddressId { get; set; }
        public string PhoneNr { get; set; }
        public int LoginId { get; set; }
    }
}
