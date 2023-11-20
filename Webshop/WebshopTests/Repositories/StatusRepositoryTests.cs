namespace WebshopTests.Repositories
{
    public class StatusRepositoryTests
    {
        private readonly DbContextOptions<DatabaseContext> _optinons;
        private readonly DatabaseContext _context;
        private readonly StatusRepository _statusRepository;
        public StatusRepositoryTests()
        {
            _optinons = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "StatusRepositoryTests")
                .Options;

            _context = new(_optinons);

            _statusRepository = new(_context);
        }
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatus_WhenStatusExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int statusId = 1;

            _context.Status.Add(new()
            {
                Id = statusId,
                Name = "Recived"
            });

            await _context.SaveChangesAsync();

            //Act
            var result = await _statusRepository.FindByIdAsync(statusId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(statusId, result.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenStatusDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int statusId = 1;

            //Act
            var result = await _statusRepository.FindByIdAsync(statusId);

            //Assert
            Assert.Null(result);
        }
    }
}
