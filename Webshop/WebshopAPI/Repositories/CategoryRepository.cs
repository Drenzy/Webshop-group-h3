namespace WebshopAPI.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> CreateAsync(Category newCategory);
        Task<Category?> FindByIdAsync(int categoryId);
        Task<Category?> UpdateByIdAsync(int categoryId, Category updateCategory);
        Task<Category?> DeleteByIdAsync(int categoryId);
    }
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DatabaseContext _context;

        //The class getting the database and set it as a private variable so it can use it in other methodes
        public CategoryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(Category newCategory)
        {
            //We are trying to add a new element to the database
            _context.Category.Add(newCategory);
            //We are saving the changes in the databse
            await _context.SaveChangesAsync();
            //We are retuning the new category
            return newCategory;
        }

        public async Task<Category?> DeleteByIdAsync(int categoryId)
        {
            var category = await FindByIdAsync(categoryId);
            if (category != null)
            {
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
            }
            return category;
        }

        public async Task<Category?> FindByIdAsync(int categoryId)
        {
            return await _context.Category.FindAsync(categoryId);
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Category.ToListAsync();
        }

        public async Task<Category?> UpdateByIdAsync(int categoryId, Category updateCategory)
        {
            var category = await FindByIdAsync(categoryId);
            if (category != null)
            {
                category.Name = updateCategory.Name;

                await _context.SaveChangesAsync();

                category = await FindByIdAsync(categoryId);
            }
            return category;
        }
    }
}
