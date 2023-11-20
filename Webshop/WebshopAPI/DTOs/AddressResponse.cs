namespace WebshopAPI.DTOs
{
    public class AddressResponse
    {
        public int Id { get; set; }
        public string StreetName { get; set; } = string.Empty;
        public int ZipCodeId { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

    }
}
