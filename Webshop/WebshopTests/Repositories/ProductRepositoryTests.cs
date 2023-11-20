namespace WebshopTests.Repositories
{
    public class ProductRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly ProductRepository _productRepository;
        public ProductRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ProductRepositoryTests")
                .Options;

            _context = new(_optinons);

            _productRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfProducts_WhenProductExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Product.Add(new Product
            {
                Id = 1,
                Name = "Blå sko",
                Description = "Den blå sko",
                Price = 200,
                CategoryId = 1
            });

            _context.Product.Add(new Product
            {
                Id = 2,
                Name = "Sort Jeans",
                Description = "De sorte Wrangler",
                Price = 199.99M,
                CategoryId = 2
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _productRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Product>>(result);
            Assert.Equal(2, result.Count);

        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfProducts_WhenNoProductExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            //Act
            var result = await _productRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Product>>(result);
            Assert.Empty(result);


        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToProduct_WhenSavingToDatabase()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Product product = new()
            {
                Name = "Blå sko",
                Description = "Den blå sko",
                Price = 200,
                CategoryId = 1
            };

            //Act
            var result = await _productRepository.CreateAsync(product);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(expectedId, result?.Id);

        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNewProduct_WhenProductIdAlreadyExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            Product product = new()
            {
                Name = "Blå sko",
                Description = "Den blå sko",
                Price = 200,
                CategoryId = 1
            };

            await _productRepository.CreateAsync(product);

            //Act
            async Task action() => await _productRepository.CreateAsync(product);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);

        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int productId = 1;

            _context.Product.Add(new()
            {
                Id = productId,
                Name = "Blå sko",
                Description = "Den blå sko",
                Price = 200,
                CategoryId = 1
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _productRepository.FindByIdAsync(productId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int productId = 1;

            //Act
            var result = await _productRepository.FindByIdAsync(productId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnUpdatedProduct_WhenProductExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int productId = 1;

            _context.Product.Add(new()
            {
                Id = productId,
                Name = "Blå sko",
                Description = "Den blå sko",
                Price = 200,
                CategoryId = 1
            });

            await _context.SaveChangesAsync();

            Product updateProduct = new()
            {
                Name = "Rettet Sko",
                Description = "Den rettede sko",
                Price = 210,
                CategoryId = 2
            };

            //Act
            var result = await _productRepository.UpdateByIdAsync(productId, updateProduct);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(updateProduct.Name, result.Name);
            Assert.Equal(updateProduct.Description, result.Description);
            Assert.Equal(updateProduct.Price, result.Price);
            Assert.Equal(updateProduct.CategoryId, result.CategoryId);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int productId = 1;

            Product updateProduct = new()
            {
                Name = "Rettet Sko",
                Description = "Den rettede sko",
                Price = 210,
                CategoryId = 2
            };

            //Act
            var result = await _productRepository.UpdateByIdAsync(productId, updateProduct);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnProduct_WhenProductIsDeleted()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int productId = 1;

            Product product = new()
            {
                Name = "Blå sko",
                Description = "Den blå sko",
                Price = 200,
                CategoryId = 1
            };

            await _productRepository.CreateAsync(product);

            //Act
            var result = await _productRepository.DeleteByIdAsync(productId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Description, result.Description);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.CategoryId, result.CategoryId);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int productId = 1;

            //Act
            var result = await _productRepository.DeleteByIdAsync(productId);

            //Assert
            Assert.Null(result);
        }
    }
}
