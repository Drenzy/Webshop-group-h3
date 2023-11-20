namespace WebshopAPI.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order> CreateAsync(Order newOrder);
        Task<Order?> FindByIdAsync(int orderId);
        Task<List<Order>> GetAllByStatusIdAsync(int statusId);
        Task<Order?> UpdateByIdAsync(int orderId, Order updateOrder);
        Task<Order?> UpdateStatusByIdAsync(int orderId, int status);
    }
    public class OrderRepository : IOrderRepository
    {
        private readonly DatabaseContext _context;

        public OrderRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<Order> CreateAsync(Order newOrder)
        {
            _context.Order.Add(newOrder);
            await _context.SaveChangesAsync();
            newOrder = await FindByIdAsync(newOrder.Id);
            return newOrder;
        }

        public async Task<Order?> FindByIdAsync(int orderId)
        {
            return await _context.Order
                .Include(o => o.Status)
                .Include(o => o.Customer)
                .ThenInclude(c => c.Login)
                .Include(o => o.Address)
                .ThenInclude(a => a.ZipCode)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Order.ToListAsync();
        }

        public async Task<List<Order>> GetAllByStatusIdAsync(int statusId)
        {
            List<Order> orderInProcess = new();
            List<Order> orders = await _context.Order.ToListAsync();
            foreach(Order order in orders)
            {
                if(order.StatusId == statusId)
                {
                    orderInProcess.Add(order);
                }
            }
            return orderInProcess;
        }

        public async Task<Order?> UpdateByIdAsync(int orderId, Order updateOrder)
        {

            var order = await FindByIdAsync(orderId);
            if (order != null)
            {
                order.AddressId = updateOrder.AddressId;

                await _context.SaveChangesAsync();

                order = await FindByIdAsync(orderId);
            }
            return order;
        }

        public async Task<Order?> UpdateStatusByIdAsync(int orderId, int status)
        {
            var order = await FindByIdAsync(orderId);
            if (order != null)
            {
                //order.AdressId = updateOrder.AdressId;
                order.StatusId = status;

                await _context.SaveChangesAsync();

                order = await FindByIdAsync(orderId);
            }
            return order;
        }
    }
}
