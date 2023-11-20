namespace WebshopAPI.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> CreateAsync(Product newProduct);
        Task<Product?> FindByIdAsync(int productId);
        Task<Product?> UpdateByIdAsync(int productId, Product updateProduct);
        Task<Product?> DeleteByIdAsync(int productId);
    }
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _context;

        //The class getting the database and set it as a private variable so it can use it in other methodes
        public ProductRepository(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<Product> CreateAsync(Product newProduct)
        {
            _context.Product.Add(newProduct);
            await _context.SaveChangesAsync();
            return newProduct;
        }

        public async Task<Product?> DeleteByIdAsync(int productId)
        {
            var product = await FindByIdAsync(productId);
            if (product != null)
            {
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
            }
            return product;
        }

        public async Task<Product?> FindByIdAsync(int productId)
        {
            return await _context.Product.FindAsync(productId);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<Product?> UpdateByIdAsync(int productId, Product updateProduct)
        {
            var product = await FindByIdAsync(productId);
            if (product != null)
            {
                product.Name = updateProduct.Name;
                product.Description = updateProduct.Description;
                product.Price = updateProduct.Price;
                product.CategoryId = updateProduct.CategoryId;

                await _context.SaveChangesAsync();

                product = await FindByIdAsync(productId);
            }
            return product;
        }
    }
}
