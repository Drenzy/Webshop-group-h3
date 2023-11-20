namespace WebshopAPI.Repositories
{
    public interface IStatusRepository
    {
        Task<Status?> FindByIdAsync(int categoryId);
        Task<List<Status>> GetAllAsync();
    }
    public class StatusRepository : IStatusRepository
    {
        private readonly DatabaseContext _context;

        //The class getting the database and set it as a private variable so it can use it in other methodes
        public StatusRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Status?> FindByIdAsync(int statusId)
        {
            return await _context.Status.FindAsync(statusId);
        }

        public async Task<List<Status>> GetAllAsync()
        {
            return await _context.Status
                .ToListAsync();
        }
    }
}
