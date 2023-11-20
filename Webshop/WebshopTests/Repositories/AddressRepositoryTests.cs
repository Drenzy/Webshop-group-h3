namespace WebshopTests.Repositories
{
    public class AddressRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly AddressRepository _addressRepository;

        public AddressRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "AddressRepositoryTests")
                .Options;

            _context = new(_options);

            _addressRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfAddresses_WhenAddressExists()
        {
            //Arrange
            // Makes sure the database is deleted
            await _context.Database.EnsureDeletedAsync();

            _context.ZipCode.Add(new ZipCode
            {
                Id = 2750,
                City = "Ballerup"
            });

            _context.Address.Add(new Address
            {
                Id = 1,
                StreetName = "Test1",
                ZipCodeId = 2750,
                Country = "Denmark",                
            });
            _context.Address.Add(new Address
            {
                Id = 2,
                StreetName = "Test2",
                ZipCodeId = 2750,
                Country = "Denmark"
            });

            // Saves the added changes 
            await _context.SaveChangesAsync();

            // Act
            var result = await _addressRepository.GetAllAsync();

            //assert
            Assert.NotNull(result);
            Assert.IsType<List<Address>>(result);
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfAddresses_WhenNoAddressesExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            //Act
            var result = await _addressRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<Address>>(result);
            Assert.Empty(result);
        }
        [Fact]
        public async void CreateAsync_ShouldAddNewIdToAddress_WhenSavingToDatabase()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Address address = new()
            {
                Id = 1,
                StreetName = "Test1",
                ZipCodeId = 2750,
                Country = "Denmark",
            };

            //Act
            var result = await _addressRepository.CreateAsync(address);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Address>(result);
            Assert.Equal(expectedId, result?.Id);

        }
        [Fact]
        public async void CreateAsync_ShouldFailToAddNewAddress_WhenAddressIdAlreadyExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            Address address = new()
            {
                StreetName = "Test1",
                ZipCodeId = 2750,
                Country = "Denmark",
            };

            await _addressRepository.CreateAsync(address);

            //Act
            async Task action() => await _addressRepository.CreateAsync(address);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);

        }
        [Fact]
        public async void FindByIdAsync_ShouldReturnAddress_WhenAddressExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int addressId = 1;

            _context.ZipCode.Add(new ZipCode
            {
                Id = 2750,
                City = "Ballerup"
            });

            _context.Address.Add(new()
            {
                Id = addressId,
                StreetName = "Test1",
                ZipCodeId = 2750,
                Country = "Denmark"
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _addressRepository.FindByIdAsync(addressId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(addressId, result.Id);
        }
        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenAddressDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int addressId = 1;

            //Act
            var result = await _addressRepository.FindByIdAsync(addressId);

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnUpdatedAddress_WhenAddressExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int addressId = 1;

            _context.ZipCode.Add(new ZipCode
            {
                Id = 2750,
                City = "Ballerup"
            });

            _context.Address.Add(new()
            {
                Id = addressId,
                StreetName = "Test1",
                ZipCodeId = 2750,
                Country = "Denmark"
            });


            await _context.SaveChangesAsync();

            Address updateaddress = new()
            {
                StreetName = "Test1Rettet",
                ZipCodeId = 2750,
                Country = "Denmark"
            };

            //Act
            var result = await _addressRepository.UpdateByIdAsync(addressId, updateaddress);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<Address>(result);
            Assert.Equal(updateaddress.StreetName, result.StreetName);
            Assert.Equal(updateaddress.Country, result.Country);
            Assert.Equal(updateaddress.ZipCodeId, result.ZipCodeId);
        }
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenAddressDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int addressId = 1;

            Address updateaddress = new()
            {
                StreetName = "Test1",
                ZipCodeId = 2750,
                Country = "Denmark",
            };

            //Act
            var result = await _addressRepository.UpdateByIdAsync(addressId, updateaddress);

            //Assert
            Assert.Null(result);
        }
    }
}
