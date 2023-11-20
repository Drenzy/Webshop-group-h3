using WebshopAPI.Authentication;

namespace WebshopTests.Repositories
{
    public class CustomerRepositoryTest
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly CustomerRepository _customerRepository;

        public CustomerRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "CustomerRepositoryTests")
                .Options;

            _context = new(_options);

            _customerRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfCustomers_WhenCustomerExists()
        {
            //Arrange
            // Makes sure the database is deleted
            await _context.Database.EnsureDeletedAsync();

            // Adding data into context which is a "copy" of the database
            _context.Customer.Add(new Customer
            {
                Id = 1,
                Name = "Test",
                PhoneNr ="12345678",
                LoginId= 1

            });
            _context.Customer.Add(new Customer
            {
                Id = 2,
                Name = "Test2",
                PhoneNr = "12345677",
                LoginId = 2
            });

            // Saves the added changes 
            await _context.SaveChangesAsync();

            // Act
            // Waiting for the task to be completed
            var result = await _customerRepository.GetAllAsync();

            //assert
            // Makes sure it is NotNUll
            Assert.NotNull(result);
            // Makes sure its the right type it is given
            Assert.IsType<List<Customer>>(result);
            // Verifying that the two the objects are eaquel / making sure there are 2 entries in the database
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOCustomers_WhenNoCustomerExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();


            // Act
            var result = await _customerRepository.GetAllAsync();

            //assert
            Assert.NotNull(result);
            Assert.IsType<List<Customer>>(result);
            // Veryfies that the collection is empty
            Assert.Empty(result);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToCustomer_WhenSavingToDatabase()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Customer customer = new()
            {
                Id = 1,
                Name = "Test",
                PhoneNr = "12345678",
                LoginId = 1
            };
            //Act
            var result = await _customerRepository.CreateAsync(customer);
            //Assert
            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
            // Verifying that the expected ID is the same as the result ID.
            Assert.Equal(expectedId, result?.Id);
        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNewCustomer_WhenCustomerIdAlreadyExists()
        {
            await _context.Database.EnsureDeletedAsync();


            Customer customer = new()
            {
                Id = 1,
                Name = "Test",
                PhoneNr = "12345678",
                LoginId = 1
            };

            await _customerRepository.CreateAsync(customer);
            //Act

            async Task action() => await _customerRepository.CreateAsync(customer);

            //Assert
            // Verifying that the error message is outputed
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnCustomer_WhenCustomerExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int CustomerId = 1;

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
                Id = CustomerId,
                Name = "Test",
                PhoneNr = "12345678",
                LoginId = 1,
                AddressId = 1
            });

            await _context.SaveChangesAsync();

            //Act
            var reusult = await _customerRepository.FindByIdAsync(CustomerId);

            //Assert
            Assert.NotNull(reusult);
            Assert.Equal(CustomerId, reusult.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;

            //Act
            var reusult = await _customerRepository.FindByIdAsync(customerId);

            // Assert
            Assert.Null(reusult);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnUpdatedCustomer_WhenCustomerExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int CustomerId = 1;

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
                Id = CustomerId,
                Name = "Test",
                PhoneNr = "12345678",
                LoginId = 1,
                AddressId = 1
            });

            await _context.SaveChangesAsync();

            Customer updateCustomer = new()
            {
                Name = "Test2",
                PhoneNr = "12345578",
            };

            var result = await _customerRepository.UpdateByIdAsync(CustomerId, updateCustomer);

            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
            Assert.Equal(updateCustomer.Name, result.Name);
            Assert.Equal(updateCustomer.PhoneNr, result.PhoneNr);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;

            Customer updateCustomer = new()
            {
                Name = "Test",
                PhoneNr = "12345678",
                LoginId = 1
            };
            var result = await _customerRepository.UpdateByIdAsync(customerId, updateCustomer);

            Assert.Null(result);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnCustomer_WhenCustomerIsDeleted()
        {
            await _context.Database.EnsureDeletedAsync();

            int CustomerId = 1;

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

            await _context.SaveChangesAsync();

            Customer customer = new()
            {
                Name = "Test",
                PhoneNr = "12345678",
                LoginId = 1,
                AddressId = 1
            };

            await _customerRepository.CreateAsync(customer);

            await _context.SaveChangesAsync();

            var result = await _customerRepository.DeleteByIdAsync(CustomerId);

            Assert.NotNull(result);
            Assert.IsType<Customer>(result);
            Assert.Equal(CustomerId, result.Id);
            Assert.Equal(customer.Name, result.Name);
            Assert.Equal(customer.PhoneNr, result.PhoneNr);
            Assert.Equal(customer.LoginId, result.LoginId);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenCustomerDoesNotExist()
        {
            await _context.Database.EnsureDeletedAsync();

            int customerId = 1;

            var result = await _customerRepository.DeleteByIdAsync(customerId);

            Assert.Null(result);
        }
    }
}
