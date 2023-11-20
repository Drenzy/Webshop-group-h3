namespace WebshopTests.Controllers
{
    public class AddressControllerTests
    {
        private readonly AddressController _addressController;
        private readonly Mock<IAddressRepository> _addressRepositoryMock = new();
        public AddressControllerTests()
        {
            _addressController = new(_addressRepositoryMock.Object);
        }
        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenAddressesExists()
        {
            //Arrange
            List<Address> address = new()
            {
                new Address
                {
                    Id = 1,
                    StreetName = "Test1",
                    ZipCodeId = 2300,
                    Country = "Denmark"
                },
                new Address
                {
                    Id = 2,
                    StreetName = "Test1",
                    ZipCodeId = 2300,
                    Country = "Denmark"
                }
            };

            _addressRepositoryMock.Setup(a => a.GetAllAsync()).ReturnsAsync(address);

            //Act
            var result = await _addressController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<AddressResponse>>(objectResult.Value);
            var data = objectResult.Value as List<AddressResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }
        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {


            _addressRepositoryMock.Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an expection"));

            //var
            var result = await _addressController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<AddressResponse>;
            Assert.Null(data);
        }
        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_WhenAddressIsSuccessfullyCreated()
        {
            //arrange


            AddressRequest addressRequest = new()
            {
                StreetName = "Test1",
                ZipCodeId = 2300,
                Country = "Denmark"
            };

            int addressId = 1;
            Address address = new()
            {
                Id = addressId,
                StreetName = "Test1",
                ZipCodeId = 2300,
                Country = "Denmark"
            };

            _addressRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Address>()))
                .ReturnsAsync(address);
            //act
            var result = await _addressController.CreateAsync(addressRequest);
            //assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as AddressResponse;
            Assert.NotNull(data);
            Assert.Equal(addressId, data.Id);
            Assert.Equal(address.StreetName, data.StreetName);
            Assert.Equal(address.Country, data.Country);
            Assert.Equal(address.ZipCodeId, data.ZipCodeId);
        }
        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //arange
            AddressRequest addressRequest = new()
            {
                StreetName = "Test1",
                ZipCodeId = 2300,
                Country = "Denmark"
            };
            _addressRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Address>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));
            //act
            var result = await _addressController.CreateAsync(addressRequest);
            //assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenAddressExists()
        {
            // Arrange
            int addressId = 1;
            AddressResponse addressResponse = new()
            {
                Id = addressId,
                StreetName = "Test1",
                ZipCodeId = 2300,
                Country = "Denmark"
            };

            Address address = new()
            {
                Id = addressId,
                StreetName = "Test1",
                ZipCodeId = 2300,
                Country = "Denmark"
            };
            _addressRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(address);

            // Act
            var result = await _addressController.FindByIdAsync(addressId);

            // Assert
            var obejctReuslt = result as ObjectResult;
            Assert.NotNull(obejctReuslt);
            Assert.Equal(200, obejctReuslt.StatusCode);

            var data = obejctReuslt.Value as AddressResponse;
            Assert.NotNull(data);
            Assert.Equal(addressId, data.Id);
            Assert.Equal(addressResponse.StreetName, data.StreetName);
            Assert.Equal(addressResponse.Country, data.Country);
            Assert.Equal(addressResponse.ZipCodeId, data.ZipCodeId);
        }
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenAddressDoesNotExist()
        {
            int addressId = 1;

            _addressRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await _addressController.FindByIdAsync(addressId);

            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }
        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            int addressId = 1;

            _addressRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(()
                => throw new Exception("This is an execption"));

            var result = await _addressController.FindByIdAsync(addressId);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

        }
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenAddressIsUpdated()
        {
            //arrange
            AddressRequest addressRequest = new()
            {
                StreetName = "Test1",
                ZipCodeId = 2300,
                Country = "Denmark"
            };

            int addressId = 1;
            Address address = new()
            {
                Id = addressId,
                StreetName = "Test1",
                ZipCodeId = 2300,
                Country = "Denmark"
            };

            _addressRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Address>()))
                .ReturnsAsync(address);


            //Act
            var result = await _addressController.UpdateByIdAsync(addressId, addressRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as AddressResponse;
            Assert.NotNull(data);
            Assert.Equal(addressId, data.Id);
            Assert.Equal(address.StreetName, data.StreetName);
            Assert.Equal(address.Country, data.Country);
            Assert.Equal(address.ZipCodeId, data.ZipCodeId);
        }
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenCustomerDoesNotExist()
        {
            //Arrange
            AddressRequest addressRequest = new()
            {
                StreetName = "Test1",
                ZipCodeId = 2300,
                Country = "Denmark"
            };

            int addressId = 1;

            _addressRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var result = await _addressController.UpdateByIdAsync(addressId, addressRequest);

            // Assert
            var objetResult = result as NotFoundResult;
            Assert.NotNull(objetResult);
            Assert.Equal(404, objetResult.StatusCode);
        }
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // arrange 
            AddressRequest addressRequest = new()
            {
                StreetName = "Test1",
                ZipCodeId = 2300,
                Country = "Denmark"
            };

            int addressId = 1;

            _addressRepositoryMock.Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Address>())).ReturnsAsync(()
                => throw new Exception("This is an execption"));

            // Act
            var result = await _addressController.UpdateByIdAsync(addressId, addressRequest);

            // Assert
            var objetResult = result as ObjectResult;
            Assert.NotNull(objetResult);
            Assert.Equal(500, objetResult.StatusCode);
        }
    }
}
