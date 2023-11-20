namespace WebshopTests.Controllers
{
    public class ZipCodeControllerTests
    {
        private readonly ZipCodeController _zipcodeController;
        private readonly Mock<IZipCodeRepository> _zipcodeRepositoryMock = new();
        public ZipCodeControllerTests()
        {
            _zipcodeController = new(_zipcodeRepositoryMock.Object);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenStatusExists()
        {
            //Arrange
            int zipcodeId = 2750;
            ZipCodeResponse zipcodeResponse = new()
            {
                Id = zipcodeId,
                City = "Ballerup"
            };
            ZipCode zipcode = new()
            {
                Id = zipcodeId,
                City = "Ballerup"
            };
            _zipcodeRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(zipcode);

            //Act
            var result = await _zipcodeController.FindByIdAsync(zipcodeId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ZipCodeResponse;
            Assert.NotNull(data);
            Assert.Equal(zipcodeId, data.Id);
            Assert.Equal(zipcode.City, data.City);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenStatusDoesNotExist()
        {
            //Arrange
            int zipcodeId = 2750;

            _zipcodeRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _zipcodeController.FindByIdAsync(zipcodeId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int zipcodeId = 2750;

            _zipcodeRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _zipcodeController.FindByIdAsync(zipcodeId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenAddressesExists()
        {
            //Arrange
            List<ZipCode> zipcode = new()
            {
                new ZipCode
                {
                    Id = 2300,
                    City = "Ballerup"
                },
                new ZipCode
                {
                    Id = 2300,
                    City = "Ballerup"
                }
            };

            _zipcodeRepositoryMock.Setup(a => a.GetAllAsync()).ReturnsAsync(zipcode);

            //Act
            var result = await _zipcodeController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<ZipCodeResponse>>(objectResult.Value);
            var data = objectResult.Value as List<ZipCodeResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }
        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {


            _zipcodeRepositoryMock.Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an expection"));

            //var
            var result = await _zipcodeController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<ZipCodeResponse>;
            Assert.Null(data);
        }
    }
}
