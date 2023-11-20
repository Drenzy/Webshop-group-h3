namespace WebshopTests.Controllers
{
    public class StatusControllerTests
    {
        private readonly StatusController _statusController;
        private readonly Mock<IStatusRepository> _statusRepositoryMock = new();
        public StatusControllerTests()
        {
            _statusController = new(_statusRepositoryMock.Object);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenStatusExists()
        {
            //Arrange
            int statusId = 1;
            StatusResponse statusResponse = new()
            {
                Id = statusId,
                Name = "Sko"
            };
            Status status = new()
            {
                Id = statusId,
                Name = "Sko"
            };
            _statusRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(status);

            //Act
            var result = await _statusController.FindByIdAsync(statusId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as StatusResponse;
            Assert.NotNull(data);
            Assert.Equal(statusId, data.Id);
            Assert.Equal(status.Name, data.Name);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenStatusDoesNotExist()
        {
            //Arrange
            int statusId = 1;

            _statusRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _statusController.FindByIdAsync(statusId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int statusId = 1;

            _statusRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _statusController.FindByIdAsync(statusId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
