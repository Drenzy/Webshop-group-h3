namespace WebshopAPI.DTOs
{
    public class CustomerResponse
    {
        public int Id { get; set; }
        public string Name { get; set;  }
        public int AddressId { get; set; }
        public string PhoneNr { get; set; }
        public int LoginId { get; set; }

            public CustomerLoginResponse Logins { get; set; }
            public CustomerAddressResponse Addresses { get; set; }
    }

    public class CustomerLoginResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Role IsAdmin { get; set; }
    }

    public class CustomerAddressResponse
    {
        public string StreetName { get; set; }
        public int ZipCodeId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
