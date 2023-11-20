namespace WebshopTests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly OrderRepository _orderRepository;

        public OrderRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "OrderRepositoryTests")
                .Options;

            _context = new(_options);

            _orderRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfOrders_WhenOrderExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            _context.Order.Add(new Order
            {
                Id = 1,
                CustomerId = 1,
                OrderDate = DateTime.Parse("2002-01-12"),
                StatusId = 4
            });

            _context.Order.Add(new Order
            {
                Id = 2,
                CustomerId = 1,
                OrderDate = DateTime.Parse("2023-01-12"),
                StatusId = 1
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _orderRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Order>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfOrders_WhenNoOrderExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            //Act
            var result = await _orderRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Order>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void GetAllByStatusIdAsync_ShouldReturnListOfOrdersWithStatus3_WhenOrderExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int statusId = 3;

            _context.Order.Add(new Order
            {
                Id = 1,
                CustomerId = 1,
                OrderDate = DateTime.Parse("2023-01-12"),
                StatusId = 4
            });

            _context.Order.Add(new Order
            {
                Id = 2,
                CustomerId = 1,
                OrderDate = DateTime.Parse("2002-01-12"),
                StatusId = 3
            });

            _context.Order.Add(new Order
            {
                Id = 3,
                CustomerId = 1,
                OrderDate = DateTime.Parse("2023-01-12"),
                StatusId = 3
            });

            await _context.SaveChangesAsync();

            //Act 
            var result = await _orderRepository.GetAllByStatusIdAsync(statusId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Order>>(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAllByStatusIdAsync_ShouldReturnEmptyListOfOrders_WhenNoOrderExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            int statusId = 1;

            //Act
            var result = await _orderRepository.GetAllByStatusIdAsync(statusId);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Order>>(result);
            Assert.Empty(result);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToOrder_WhenSavingToDatabase()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            _context.ZipCode.Add(new ZipCode
            {
                Id = 2750,
                City = "Ballerup"
            });

            _context.Address.Add(new()
            {
                Id = 1,
                StreetName = "Test1",
                ZipCodeId = 2750,
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

            _context.Customer.Add(new Customer
            {
                Id = 1,
                Name = "Test",
                PhoneNr = "12345678",
                LoginId = 1

            });
            _context.Status.Add(new()
            {
                Id = 1,
                Name = "Recived"
            });

            await _context.SaveChangesAsync();

            Order order = new()
            {
                CustomerId = 1,
                AddressId = 1,
                OrderDate = DateTime.Now,
                StatusId = 1
            };

            //Act
            var result = await _orderRepository.CreateAsync(order);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Order>(result);
            Assert.Equal(expectedId, result?.Id);

        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNeworder_WhenOrderIdAlreadyExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            Order order = new()
            {
                CustomerId = 1,
                OrderDate = DateTime.Parse("2002-01-12"),
                StatusId = 4
            };

            await _orderRepository.CreateAsync(order);

            //Act
            async Task action() => await _orderRepository.CreateAsync(order);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);

        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int orderId = 1;

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

            await _context.SaveChangesAsync();

            _context.Order.Add(new()
            {
                Id = orderId,
                CustomerId = 1,
                OrderDate = DateTime.Parse("2002-01-12"),
                StatusId = 1,
                AddressId = 1
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _orderRepository.FindByIdAsync(orderId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(orderId, result.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int orderId = 1;

            //Act
            var result = await _orderRepository.FindByIdAsync(orderId);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnUpdatedOrder_WhenOrderExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int orderId = 1;

            _context.ZipCode.Add(new()
            {
                Id = 2300,
                City = "København S"
            });

            _context.ZipCode.Add(new()
            {
                Id = 2750,
                City = "Ballerup"
            });

            _context.Address.Add(new()
            {
                Id = 1,
                StreetName = "Telegrafvej 9",
                ZipCodeId = 2300,
                Country = "Denmark"
            });

            _context.Address.Add(new()
            {
                Id = 2,
                StreetName = "Telegrafvej 7",
                ZipCodeId = 2750,
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

            await _context.SaveChangesAsync();

            _context.Order.Add(new()
            {
                Id = orderId,
                CustomerId = 1,
                OrderDate = DateTime.Parse("2002-01-12"),
                StatusId = 1,
                AddressId = 1
            });

            await _context.SaveChangesAsync();

            Order updateOrder = new()
            {
                AddressId = 2
            };

            //Act
            var result = await _orderRepository.UpdateByIdAsync(orderId, updateOrder);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Order>(result);
            Assert.Equal(updateOrder.AddressId, result.AddressId);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int orderId = 1;

            Order updateorder = new()
            {
                AddressId = 2
            };

            //Act
            var result = await _orderRepository.UpdateByIdAsync(orderId, updateorder);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateStatusByIdAsync_ShouldReturnUpdatedOrder_WhenOrderExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int orderId = 1;

            _context.Status.Add(new()
            {
                Id = 2,
                Name = "Recived"
            });

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

            await _context.SaveChangesAsync();

            _context.Order.Add(new()
            {
                Id = orderId,
                CustomerId = 1,
                OrderDate = DateTime.Parse("2002-01-12"),
                StatusId = 1,
                AddressId = 1
            });

            await _context.SaveChangesAsync();

            int status = 2;

            //Act
            var result = await _orderRepository.UpdateStatusByIdAsync(orderId, status);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Order>(result);
            Assert.Equal(status, result.StatusId);
        }

        [Fact]
        public async void UpdateStatusByIdAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int orderId = 1;

            int status = 1;

            //Act
            var result = await _orderRepository.UpdateStatusByIdAsync(orderId, status);

            //Assert
            Assert.Null(result);
        }

    }
}
