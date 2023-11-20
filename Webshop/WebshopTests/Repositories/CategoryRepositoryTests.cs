namespace WebshopTests.Repositories
{
    public class CategoryRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly CategoryRepository _categoryRepository;
        public CategoryRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "CategoryRepositoryTests")
                .Options;

            _context = new(_optinons);

            _categoryRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfCategorys_WhenCategoryExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Category.Add(new Category
            {
                Id = 1,
                Name = "Sko"
            });

            _context.Category.Add(new Category
            {
                Id = 2,
                Name = "Bukser"
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _categoryRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfCategorys_WhenNoCategoryExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            //Act
            var result = await _categoryRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Category>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToCategory_WhenSavingToDatabase()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Category category = new()
            {
                Name = "Sko"
            };

            //Act
            var result = await _categoryRepository.CreateAsync(category);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(expectedId, result?.Id);

        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNewcategory_WhenCategoryIdAlreadyExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            Category category = new()
            {
                Name = "Sko"
            };

            await _categoryRepository.CreateAsync(category);

            //Act
            async Task action() => await _categoryRepository.CreateAsync(category);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);

        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnCategory_WhenCategoryExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int categoryId = 1;

            _context.Category.Add(new()
            {
                Id = categoryId,
                Name = "Sko"
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _categoryRepository.FindByIdAsync(categoryId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(categoryId, result.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int categoryId = 1;

            //Act
            var result = await _categoryRepository.FindByIdAsync(categoryId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnUpdatedCategory_WhenCategoryExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int categoryId = 1;

            _context.Category.Add(new()
            {
                Id = categoryId,
                Name = "Sko"
            });

            await _context.SaveChangesAsync();

            Category updatecategory = new()
            {
                Name = "Rettet sko"
            };

            //Act
            var result = await _categoryRepository.UpdateByIdAsync(categoryId, updatecategory);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(updatecategory.Name, result.Name);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int categoryId = 1;

            Category updatecategory = new()
            {
                Name = "Rettet sko"
            };

            //Act
            var result = await _categoryRepository.UpdateByIdAsync(categoryId, updatecategory);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnCategory_WhenCategoryIsDeleted()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int categoryId = 1;

            Category category = new()
            {
                Name = "Sko"
            };

            await _categoryRepository.CreateAsync(category);

            //Act
            var result = await _categoryRepository.DeleteByIdAsync(categoryId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(categoryId, result.Id);
            Assert.Equal(category.Name, result.Name);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenCategoryDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int categoryId = 1;

            //Act
            var result = await _categoryRepository.DeleteByIdAsync(categoryId);

            //Assert
            Assert.Null(result);
        }
    }
}
