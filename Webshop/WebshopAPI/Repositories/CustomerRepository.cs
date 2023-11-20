using WebshopAPI.Entities;

namespace WebshopAPI.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer> CreateAsync(Customer newCustomer);
        Task<Customer> FindByIdAsync(int customerId);
        Task<Customer> UpdateByIdAsync(int customerId, Customer updateCustomer);
        Task<Customer> DeleteByIdAsync(int customerId);
    }
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DatabaseContext _context;

        public CustomerRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customer.ToListAsync();
        }

        public async Task<Customer> CreateAsync(Customer newCustomer)
        {
            _context.Customer.Add(newCustomer);
            await _context.SaveChangesAsync();
            return newCustomer;
        }

        public async Task<Customer?> FindByIdAsync(int customerId)
        {
            return await _context.Customer
            .Include(c => c.Login)
            .Include(c => c.Address)
            .ThenInclude(a => a.ZipCode)
            .FirstOrDefaultAsync(c => c.Id == customerId);
        }

        public async Task<Customer?> UpdateByIdAsync(int customerId, Customer updateCustomer)
        {
            var customer = await FindByIdAsync(customerId);
            if (customer != null)
            {
                customer.Name= updateCustomer.Name;
                customer.PhoneNr= updateCustomer.PhoneNr;

                await _context.SaveChangesAsync();

                // increase entity has foreingkeys, get the updated data
                // not really nessasary but, will be when closet is added.
                customer = await FindByIdAsync(customerId);
            }
            return customer;
        }

        public async Task<Customer?> DeleteByIdAsync(int customerId)
        {
            var customer = await FindByIdAsync(customerId);
            if (customer != null)
            {
                _context.Customer.Remove(customer);
                await _context.SaveChangesAsync();
            }
            return customer;
        }
    }
}
