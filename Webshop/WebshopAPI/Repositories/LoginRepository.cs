namespace WebshopAPI.Repositories
{

    public interface ILoginRepository
    {
        Task<List<Login>> GetAllAsync();
        Task<Login> CreateAsync(Login newLogin);
        Task<Login> FindByIdAsync(int loginId);
        Task<Login> UpdateByIdAsync(int loginId, Login updateLogin);
        Task<Login> DeleteByIdAsync(int loginId);
        Task<Login?> GetByEmail(string email);
    }
    public class LoginRepository : ILoginRepository
    {

        private readonly DatabaseContext _context;

        public LoginRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Login?> CreateAsync(Login login)
        {
            _context.Login.Add(login);
            await _context.SaveChangesAsync();
            login = await FindByIdAsync(login.Id);
            return login;
        }

        public async Task<Login?> DeleteByIdAsync(int loginId)
        {
            var login = await FindByIdAsync(loginId);
            if (login != null)
            {
                _context.Login.Remove(login);
                await _context.SaveChangesAsync();
            }
            return login;
        }

        public async Task<Login?> FindByIdAsync(int loginId)
        {

            return await _context.Login
            .Include(l => l.Customer)
            .ThenInclude(l => l.Address)
            .ThenInclude(a => a.ZipCode)
            .FirstOrDefaultAsync(c => c.Id == loginId);

        }

        public async Task<List<Login?>> GetAllAsync()
        {
            return await _context.Login.ToListAsync();
        }

        public async Task<Login?> GetByEmail(string email)
        {
            return await _context.Login
                .Include(c => c.Customer)
                .ThenInclude(a => a.Address)
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<Login?> UpdateByIdAsync(int loginId, Login updateLogin)
        {
            var login = await FindByIdAsync(loginId);
            if (login != null)
            {
                login.UserName = updateLogin.UserName;
                login.Email = updateLogin.Email;
                login.Role = updateLogin.Role;
                if(updateLogin.Password != null && updateLogin.Password != "")
                {
                    login.Password = updateLogin.Password;
                }

                await _context.SaveChangesAsync();

                // increase entity has foreingkeys, get the updated data
                // not really nessasary but, will be when closet is added.
                login = await FindByIdAsync(loginId);
            }
            return login;
        }


    }
}
