namespace WebshopTests.Repositories
{
    public class ZipCodeRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly ZipcodeRepository _zipcodeRepository;

        public ZipCodeRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "ZipCodeRepositoryTests")
                .Options;

            _context = new(_optinons);

            _zipcodeRepository = new(_context);
        }
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatus_WhenZipCodeExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int zipcodeId = 2750;

            _context.ZipCode.Add(new()
            {
                Id = zipcodeId,
                City = "Ballerup"
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _zipcodeRepository.FindByIdAsync(zipcodeId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(zipcodeId, result.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenZipCodeDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int zipcodeId = 1;

            //Act
            var result = await _zipcodeRepository.FindByIdAsync(zipcodeId);

            //Assert
            Assert.Null(result);
        }
        [Fact]
        public async void GetAllAsync_ShouldReturnListOfZipCodes_WhenZipcodeExists()
        {
            //Arrange
            // Makes sure the database is deleted
            await _context.Database.EnsureDeletedAsync();

            _context.ZipCode.Add(new ZipCode
            {
                Id = 2750,
                City = "Ballerup"
            });
            _context.ZipCode.Add(new ZipCode
            {
                Id = 2300,
                City = "Ballerup"
            });

            // Saves the added changes 
            await _context.SaveChangesAsync();

            // Act
            var result = await _zipcodeRepository.GetAllAsync();

            //assert
            Assert.NotNull(result);
            Assert.IsType<List<ZipCode>>(result);
            Assert.Equal(2, result.Count);
        }
        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfAddresses_WhenNoAddressesExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            await _context.SaveChangesAsync();

            //Act
            var result = await _zipcodeRepository.GetAllAsync();

            //Assert
            Assert.NotNull(result);
            Assert.IsType<List<ZipCode>>(result);
            Assert.Empty(result);
        }
    }
}
