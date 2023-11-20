namespace WebshopAPI.Repositories
{
    public interface IZipCodeRepository
    {
        Task<ZipCode?> FindByIdAsync(int zipcodeId);
        Task<List<ZipCode>> GetAllAsync();
    }
    public class ZipcodeRepository : IZipCodeRepository
    {
        private readonly DatabaseContext _context;

        public ZipcodeRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ZipCode?> FindByIdAsync(int zipcodeId)
        {
            return await _context.ZipCode.FindAsync(zipcodeId);
        }
        public async Task<List<ZipCode>> GetAllAsync()
        {
            return await _context.ZipCode
                .ToListAsync();
        }
    }
}
