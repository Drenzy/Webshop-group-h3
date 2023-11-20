namespace WebshopAPI.Repositories
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetAllAsync();

        Task<Address> CreateAsync(Address adress);
        Task<Address> FindByIdAsync(int adressId);
        Task<Address> UpdateByIdAsync(int adressId, Address updateAddress);

    }
    public class AddressRepository : IAddressRepository
    {
        private readonly DatabaseContext _context;

        public AddressRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Address> CreateAsync(Address adress)
        {
            _context.Address.Add(adress);

            await _context.SaveChangesAsync();

            return adress;
        }

        public async Task<Address> FindByIdAsync(int adressId)
        {
            return await _context.Address
                .Include(a => a.ZipCode)
                .FirstOrDefaultAsync(a => a.Id == adressId);
        }

        public async Task<List<Address>> GetAllAsync()
        {
            return await _context.Address
                .Include(a => a.ZipCode)
                .ToListAsync();
        }

        public async Task<Address> UpdateByIdAsync(int adressId, Address updateAddress)
        {
            var adress = await FindByIdAsync(adressId);
            if(adress != null) 
            { 
                adress.StreetName = updateAddress.StreetName;
                adress.ZipCodeId = updateAddress.ZipCodeId;
                adress.Country = updateAddress.Country;

                await _context.SaveChangesAsync();

                adress = await FindByIdAsync(adressId);
            }
            return adress;
        }

    }
}
