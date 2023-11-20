namespace WebshopAPI.Repositories
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> CreateAsync(OrderItem newOrderItem);
        Task<OrderItem?> FindByIdAsync(int orderItemId);
        Task<OrderItem?> UpdateByIdAsync(int orderItemId, OrderItem updateOrderItem);
        Task<OrderItem?> DeleteByIdAsync(int orderItemId);
    }
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly DatabaseContext _context;

        //The class getting the database and set it as a private variable so it can use it in other methodes
        public OrderItemRepository (DatabaseContext context)
        {
            _context = context;
        }

        public async Task<OrderItem> CreateAsync(OrderItem newOrderItem)
        {
            //We are trying to add a new element to the database
            _context.OrderItem.Add(newOrderItem);
            //We are saving the changes in the databse
            await _context.SaveChangesAsync();
            //We are retuning the new orderItem
            return newOrderItem;
        }

        public async Task<OrderItem?> DeleteByIdAsync(int orderItemId)
        {
            var orderItem = await FindByIdAsync(orderItemId);
            if (orderItem != null)
            {
                _context.OrderItem.Remove(orderItem);
                await _context.SaveChangesAsync();
            }
            return orderItem;
        }

        public async Task<OrderItem?> FindByIdAsync(int orderItemId)
        {
            return await _context.OrderItem
               .Include(oi => oi.Product)
               .FirstOrDefaultAsync(o => o.Id == orderItemId);
        }

        public async Task<OrderItem?> UpdateByIdAsync(int orderItemId, OrderItem updateOrderItem)
        {
            var orderItem = await FindByIdAsync(orderItemId);
            if (orderItem != null)
            {
                orderItem.Quantity = updateOrderItem.Quantity;

                await _context.SaveChangesAsync();

                orderItem = await FindByIdAsync(orderItemId);
            }
            return orderItem;
        }
    }
}
