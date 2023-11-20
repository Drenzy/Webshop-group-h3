namespace WebshopTests.Repositories
{
    public class OrderItemRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly OrderItemRepository _orderItemRepository;
        public OrderItemRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "OrderItemRepositoryTests")
                .Options;

            _context = new(_optinons);

            _orderItemRepository = new(_context);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToOrderItem_WhenSavingToDatabase()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            OrderItem orderItem = new()
            {
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1,
            };

            //Act
            var result = await _orderItemRepository.CreateAsync(orderItem);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OrderItem>(result);
            Assert.Equal(expectedId, result?.Id);

        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNeworderItem_WhenOrderItemIdAlreadyExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            OrderItem orderItem = new()
            {
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1,
            };

            await _orderItemRepository.CreateAsync(orderItem);

            //Act
            async Task action() => await _orderItemRepository.CreateAsync(orderItem);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);

        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnOrderItem_WhenOrderItemExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int orderItemId = 1;

            _context.ZipCode.Add(new()
            {
                Id = 2300,
                City = "København S"
            });

            _context.Address.Add(new()
            {
                Id = 1,
                StreetName = "Telegrafvej 9",
                ZipCodeId = 2300,
                Country = "Denmark"
            });

            _context.Login.Add(new()
            {
                Id = 1,
                UserName = "Test",
                Password = "Test",
                Email = "Test",
                Role = Role.Admin
            });

            _context.Customer.Add(new()
            {
                Id = 1,
                Name = "Test",
                PhoneNr = "12345678",
                LoginId = 1,
                AddressId = 1
            });

            _context.Status.Add(new()
            {
                Id = 1,
                Name = "Recived"
            });

            _context.Category.Add(new()
            {
                Id = 1,
                Name = "Sko"
            });

            _context.Product.Add(new()
            {
                Id = 1,
                Name = "Blå sko",
                Description = "Den blå sko",
                Price = 200,
                CategoryId = 1
            });

            await _context.SaveChangesAsync();

            _context.OrderItem.Add(new()
            {
                Id = orderItemId,
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1,
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _orderItemRepository.FindByIdAsync(orderItemId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(orderItemId, result.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenOrderItemDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int orderItemId = 1;

            //Act
            var result = await _orderItemRepository.FindByIdAsync(orderItemId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnUpdatedOrderItem_WhenOrderItemExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int orderItemId = 1;

            _context.ZipCode.Add(new()
            {
                Id = 2300,
                City = "København S"
            });

            _context.Address.Add(new()
            {
                Id = 1,
                StreetName = "Telegrafvej 9",
                ZipCodeId = 2300,
                Country = "Denmark"
            });

            _context.Login.Add(new()
            {
                Id = 1,
                UserName = "Test",
                Password = "Test",
                Email = "Test",
                Role = Role.Admin
            });

            _context.Customer.Add(new()
            {
                Id = 1,
                Name = "Test",
                PhoneNr = "12345678",
                LoginId = 1,
                AddressId = 1
            });

            _context.Status.Add(new()
            {
                Id = 1,
                Name = "Recived"
            });

            _context.Category.Add(new()
            {
                Id = 1,
                Name = "Sko"
            });

            _context.Product.Add(new()
            {
                Id = 1,
                Name = "Blå sko",
                Description = "Den blå sko",
                Price = 200,
                CategoryId = 1
            });

            await _context.SaveChangesAsync();

            _context.OrderItem.Add(new()
            {
                Id = orderItemId,
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1,
            });

            await _context.SaveChangesAsync();

            OrderItem updateorderItem = new()
            {
                Id = orderItemId,
                Price = 20,
                Quantity = 5,
                OrderId = 1,
                ProductId = 1,
            };

            //Act
            var result = await _orderItemRepository.UpdateByIdAsync(orderItemId, updateorderItem);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OrderItem>(result);
            Assert.Equal(updateorderItem.Price, result.Price);
            Assert.Equal(updateorderItem.Quantity, result.Quantity);
            Assert.Equal(updateorderItem.OrderId, result.OrderId);
            Assert.Equal(updateorderItem.ProductId, result.ProductId);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenOrderItemDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int orderItemId = 1;

            OrderItem updateorderItem = new()
            {
                Price = 20,
                Quantity = 5,
                OrderId = 1,
                ProductId = 1,
            };

            //Act
            var result = await _orderItemRepository.UpdateByIdAsync(orderItemId, updateorderItem);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnOrderItem_WhenOrderItemIsDeleted()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int orderItemId = 1;

            _context.ZipCode.Add(new()
            {
                Id = 2300,
                City = "København S"
            });

            _context.Address.Add(new()
            {
                Id = 1,
                StreetName = "Telegrafvej 9",
                ZipCodeId = 2300,
                Country = "Denmark"
            });

            _context.Login.Add(new()
            {
                Id = 1,
                UserName = "Test",
                Password = "Test",
                Email = "Test",
                Role = Role.Admin
            });

            _context.Customer.Add(new()
            {
                Id = 1,
                Name = "Test",
                PhoneNr = "12345678",
                LoginId = 1,
                AddressId = 1
            });

            _context.Status.Add(new()
            {
                Id = 1,
                Name = "Recived"
            });

            _context.Category.Add(new()
            {
                Id = 1,
                Name = "Sko"
            });

            _context.Product.Add(new()
            {
                Id = 1,
                Name = "Blå sko",
                Description = "Den blå sko",
                Price = 200,
                CategoryId = 1
            });

            await _context.SaveChangesAsync();

            OrderItem orderItem = new()
            {
                Price = 20,
                Quantity = 5,
                OrderId = 1,
                ProductId = 1,
            };

            await _orderItemRepository.CreateAsync(orderItem);

            //Act
            var result = await _orderItemRepository.DeleteByIdAsync(orderItemId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<OrderItem>(result);
            Assert.Equal(orderItemId, result.Id);
            Assert.Equal(orderItem.Price, result.Price);
            Assert.Equal(orderItem.Quantity, result.Quantity);
            Assert.Equal(orderItem.OrderId, result.OrderId);
            Assert.Equal(orderItem.ProductId, result.ProductId);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenOrderItemDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int orderItemId = 1;

            //Act
            var result = await _orderItemRepository.DeleteByIdAsync(orderItemId);

            //Assert
            Assert.Null(result);
        }
    }
}
