namespace WebshopTests.Controllers
{
    public class CustomerControllerTest
    {
        private readonly CustomerController _customerController;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock = new();

        public CustomerControllerTest()
        {
            _customerController = new(_customerRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenCustomersExists()
        {
            // Making a list of the data in our database
            List<Customer> customer = new()
            {
                new Customer
                {
                    Id = 1,
                    Name= "test",
                    PhoneNr = "12345678",
                    LoginId = 1
                },
                new Customer
                {
                    Id = 2,
                    Name= "test2",
                    PhoneNr = "12345578",
                    LoginId = 2
                },
            };
            // Returns the logins list
            _customerRepositoryMock.Setup(a => a.GetAllAsync()).ReturnsAsync(customer);

            //var
            var result = await _customerController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            //Returns the status code 200 if true
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            // verying that the list login is the right type
            Assert.IsType<List<CustomerResponse>>(objectResult.Value);

            // insert the objectResult into List
            var data = objectResult.Value as List<CustomerResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {


            _customerRepositoryMock.Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an expection"));

            //var
            var result = await _customerController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<LoginResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_WhenCustomerIsSuccessfullyCreated()
        {
            //arrange


            CustomerRequest customerRequest = new()
            {
                Name = "test",
                PhoneNr = "12345678",
                LoginId = 1
            };

            int customerId = 1;
            Customer customer = new()
            {
                Id = customerId,
                Name = "test1",
                PhoneNr = "12345578",
                LoginId = 2
            };

            _customerRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(customer);
            //act
            var result = await _customerController.CreateAsync(customerRequest);
            //assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as CustomerResponse;
            Assert.NotNull(data);
            Assert.Equal(customerId, data.Id);
            Assert.Equal(customer.Name, data.Name);
            Assert.Equal(customer.PhoneNr, data.PhoneNr);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //arange
            CustomerRequest customerRequest = new()
            {
                Name = "test",
                PhoneNr = "12345678",
                LoginId = 1
            };
            _customerRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Customer>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));
            //act
            var result = await _customerController.CreateAsync(customerRequest);
            //assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenCustomerExists()
        {
            // Arrange
            int customerId = 1;
            CustomerResponse customerResponse = new()
            {
                Id = customerId,
                Name = "test",
                PhoneNr = "12345678",
                LoginId = 1
            };

            Customer customer = new()
            {
                Id = customerId,
                Name = "test",
                PhoneNr = "12345678",
                LoginId = 1
            };
            _customerRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(customer);

            // Act
            var result = await _customerController.FindByIdAsync(customerId);

            // Assert
            var obejctReuslt = result as ObjectResult;
            Assert.NotNull(obejctReuslt);
            Assert.Equal(200, obejctReuslt.StatusCode);

            var data = obejctReuslt.Value as CustomerResponse;
            Assert.NotNull(data);
            Assert.Equal(customerId, data.Id);
            Assert.Equal(customerResponse.Name, data.Name);
            Assert.Equal(customerResponse.PhoneNr, data.PhoneNr);
            Assert.Equal(customerResponse.LoginId, data.LoginId);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenCustomerDoesNotExist()
        {
            int customerId = 1;

            _customerRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await _customerController.FindByIdAsync(customerId);

            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            int custoemrId = 1;

            _customerRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(()
                => throw new Exception("This is an execption"));

            var result = await _customerController.FindByIdAsync(custoemrId);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

        }
        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenCustomerIsUpdated()
        {
            //arrange
            CustomerRequest customerRequest = new()
            {
                Name = "test",
                PhoneNr = "12345678",
                LoginId = 1
            };

            int customerId = 1;
            Customer customer = new()
            {
                Id = customerId,
                Name = "test",
                PhoneNr = "12345678",
                LoginId = 1
            };
            _customerRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Customer>()))
                .ReturnsAsync(customer);


            //Act
            var result = await _customerController.UpdateByIdAsync(customerId, customerRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as CustomerResponse;
            Assert.NotNull(data);
            Assert.Equal(customerRequest.Name, data.Name);
            Assert.Equal(customerRequest.PhoneNr, data.PhoneNr);
            Assert.Equal(customerRequest.LoginId, data.LoginId);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenCustomerDoesNotExist()
        {
            //Arrange
            CustomerRequest customerRequest = new()
            {
                Name = "test",
                PhoneNr = "12345678",
                LoginId = 1
            };

            int customerId = 1;

            _customerRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var result = await _customerController.UpdateByIdAsync(customerId, customerRequest);

            // Assert
            var objetResult = result as NotFoundResult;
            Assert.NotNull(objetResult);
            Assert.Equal(404, objetResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // arrange 
            CustomerRequest customerRequest = new()
            {
                Name = "test",
                PhoneNr = "12345678",
                LoginId = 1
            };

            int CustomerId = 1;

            _customerRepositoryMock.Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Customer>())).ReturnsAsync(()
                => throw new Exception("This is an execption"));

            // Act
            var result = await _customerController.UpdateByIdAsync(CustomerId, customerRequest);

            // Assert
            var objetResult = result as ObjectResult;
            Assert.NotNull(objetResult);
            Assert.Equal(500, objetResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenLoginIsDeleted()
        {
            int customerId = 1;

            Customer customer = new()
            {
                Id = customerId,
                Name = "test",
                PhoneNr = "12345678",
                LoginId = 1
            };

            _customerRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(customer);

            //act
            var result = await _customerController.DeleteByIdAsync(customerId);

            //assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as CustomerResponse;
            Assert.NotNull(data);
            Assert.Equal(customerId, data.Id);
            Assert.Equal(customer.Name, data.Name);
            Assert.Equal(customer.PhoneNr, data.PhoneNr);
            Assert.Equal(customer.LoginId, data.LoginId);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenLoginDoesNotExist()
        {
            int customerId = 1;

            _customerRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            //act
            var result = await _customerController.DeleteByIdAsync(customerId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            int loginId = 1;

            _customerRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));
        }
    }
}
