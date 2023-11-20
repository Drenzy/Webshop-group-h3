namespace WebshopTests.Controllers
{
    public class LoginControllerTest
    {
        private readonly LoginController _loginController;
        private readonly Mock<ILoginRepository> _loginRepositoryMock = new();
        private readonly Mock<IJwtUtils> _JwtUtilsMock = new();


        public LoginControllerTest()
        {
            // need to fix this
            _loginController = new(_loginRepositoryMock.Object, _JwtUtilsMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode200_WhenLoginssExists()
        {
            // Making a list of the data in our database
            List<Login> logins = new()
            {
                new Login
                {
                    Id = 1,
                    UserName= "Test",
                    Password = "Test",
                    Email= "Test",
                    Role= Role.Admin
                },
                new Login
                {
                    Id = 2,
                    UserName= "Test1",
                    Password = "Test1",
                    Email= "Test1",
                    Role= Role.User
                },
            };
            // Returns the logins list
            _loginRepositoryMock.Setup(a => a.GetAllAsync()).ReturnsAsync(logins);

            //var
            var result = await _loginController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            //Returns the status code 200 if true
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            // verying that the list login is the right type
            Assert.IsType<List<LoginResponse>>(objectResult.Value);

            // insert the objectResult into List
            var data = objectResult.Value as List<LoginResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {


            _loginRepositoryMock.Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an expection"));

            //var
            var result = await _loginController.GetAllAsync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<LoginResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void SignInAsync_ShouldReturnStatusCode200_WhenLoginIsSuccessfullyCreated()
        {
            //arrange
            SignInRequest signInRequest = new()
            {
                Password = "Test",
                Email = "Test",
            };

            int loginId = 1;
            Login login = new()
            {
                Id = loginId,
                UserName = "Test",
                Password = BCrypt.Net.BCrypt.HashPassword("Test"),
                Email = "Test",
                Role = Role.User
            };

            _loginRepositoryMock
                .Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(login);
            //act
            var result = await _loginController.SignInAsync(signInRequest);
            //assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as SignInResponse;
            Assert.NotNull(data);
            Assert.Equal(login.Role, data.Role);
        }

        [Fact]
        public async void SignInAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //arange
            SignInRequest signInRequest = new()
            {
                Password = "Test",
                Email = "Test",
            };
            _loginRepositoryMock
                .Setup(x => x.GetByEmail(It.IsAny<string>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));
            //act
            var result = await _loginController.SignInAsync(signInRequest);
            //assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void CreateLoginWIthCustomerAsync_ShouldReturnStatusCode200_WhenLoginIsSuccessfullyCreated()
        {
            //arrange


            CustomerRegisterRequest customerRegisterRequest = new()
            {
                UserName = "Test",
                Password = "Test",
                Email = "Test",
                Name= "Test",
                PhoneNr= "Test",
                StreetName= "Test",
                ZipCode = 2300,
                Country = "danmark"
            };

            int loginId = 1;
            Login login = new()
            {
                Id = loginId,
                UserName = "Test1",
                Password = "Test1",
                Email = "Test1",
                Role = Role.User,
                Customer = new Customer
                {
                    Id = 1,
                    Name = "Test",
                    PhoneNr = "Test",
                    LoginId = loginId,
                    Address = new Address
                    {
                        Id = 1,
                        StreetName = "Test",
                        Country = "test",
                        ZipCode = new ZipCode
                        {
                            Id = 2300,
                            City = "Test"
                        }
                    }
                }
            };

            _loginRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Login>()))
                .ReturnsAsync(login);

            //act
            var result = await _loginController.CreateCustomerAsync(customerRegisterRequest);
            //assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as LoginResponse;
            Assert.NotNull(data);
            Assert.Equal(loginId, data.Id);
            Assert.Equal(login.UserName, data.UserName);
            Assert.Equal(login.Email, data.Email);
            Assert.Equal(login.Role, data.Role);
            Assert.Equal(login.Customer.Id, data.Customer.Id);
            Assert.Equal(login.Customer.Name, data.Customer.Name);
            Assert.Equal(login.Customer.PhoneNr, data.Customer.PhoneNr);
            Assert.Equal(login.Customer.Address.Id, data.Address.Id);
            Assert.Equal(login.Customer.Address.StreetName, data.Address.StreetName);
            Assert.Equal(login.Customer.Address.Country, data.Address.Country);
            Assert.Equal(login.Customer.Address.ZipCodeId, data.Address.ZipCodeId);

        }

        [Fact]
        public async void CreateCustomerAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //arange
            CustomerRegisterRequest createCustomerReqruest = new()
            {
                UserName = "Test",
                Password = "Test",
                Email = "Test",
                Name = "Test",
                PhoneNr = "Test",
                StreetName = "Test",
                ZipCode = 2300,
                Country = "danmark"
            };
            _loginRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Login>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));
            //act
            var result = await _loginController.CreateCustomerAsync(createCustomerReqruest);
            //assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenLoginExists()
        {
            // Arrange
            int loginId = 1;
            LoginResponse loginResponse = new()
            {
                Id = loginId,
                UserName = "Test",
                Email = "Test",
                Role = Role.Admin
            };

            Login login = new()
            {
                Id = loginId,
                UserName = "Test",
                Email = "Test",
                Role = Role.Admin
            };
            _loginRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(login);

            // Act
            var result = await _loginController.FindByIdAsync(loginId);

            // Assert
            var obejctReuslt = result as ObjectResult;
            Assert.NotNull(obejctReuslt);
            Assert.Equal(200, obejctReuslt.StatusCode);

            var data = obejctReuslt.Value as LoginResponse;
            Assert.NotNull(data);
            Assert.Equal(loginId, data.Id);
            Assert.Equal(loginResponse.UserName, data.UserName);
            Assert.Equal(loginResponse.Email, data.Email);
            Assert.Equal(loginResponse.Role, data.Role);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenLoginDoesNotExist()
        {
            int loginId = 1;

            _loginRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            var result = await _loginController.FindByIdAsync(loginId);

            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            int loginId = 1;

            _loginRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(()
                => throw new Exception("This is an execption"));

            var result = await _loginController.FindByIdAsync(loginId);

            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenLoginIsUpdated()
        {
            //arrange
            LoginRequest loginRequest = new()
            {
                UserName = "test",
                Email = "Test",
                Password= "Test",
                Role = Role.Admin
            };

            int loginId = 1;
            Login login = new()
            {
                Id = loginId,
                UserName = "Test",
                Email = "Test",
                Role = Role.Admin
            };
            _loginRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Login>()))
                .ReturnsAsync(login);


            //Act
            var result = await _loginController.UpdateByIdAsync(loginId, loginRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as LoginResponse;
            Assert.NotNull(data);
            Assert.Equal(loginRequest.Email, data.Email);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenLoginDoesNotExist()
        {
            //Arrange
            LoginRequest loginRequest = new()
            {
                UserName = "test",
                Email = "Test",
                Password = "Test",
                Role = Role.Admin
            };

            int loginId = 1;

            _loginRepositoryMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(() => null);

            //Act
            var result = await _loginController.UpdateByIdAsync(loginId, loginRequest);

            // Assert
            var objetResult = result as NotFoundResult;
            Assert.NotNull(objetResult);
            Assert.Equal(404, objetResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            // arrange 
            LoginRequest loginRequest = new()
            {
                UserName = "test",
                Email = "Test",
                Password = "Test",
                Role = Role.Admin
            };

            int loginId = 1;

            _loginRepositoryMock.Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Login>())).ReturnsAsync(()
                => throw new Exception("This is an execption"));

            // Act
            var result = await _loginController.UpdateByIdAsync(loginId, loginRequest);

            // Assert
            var objetResult = result as ObjectResult;
            Assert.NotNull(objetResult);
            Assert.Equal(500, objetResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenLoginIsDeleted()
        {
            int loginId = 1;

            Login login = new()
            {
                Id = loginId,
                UserName = "Test",
                Email = "Test",
                Role = Role.Admin
            };

            _loginRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(login);

            //act
            var result = await _loginController.DeleteByIdAsync(loginId);

            //assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as LoginResponse;
            Assert.NotNull(data);
            Assert.Equal(loginId, data.Id);
            Assert.Equal(login.UserName, data.UserName);
            Assert.Equal(login.Email, data.Email);
            Assert.Equal(login.Role, data.Role);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenLoginDoesNotExist()
        {
            int loginId = 1;

            _loginRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);
            //act
            var result = await _loginController.DeleteByIdAsync(loginId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            int loginId = 1;

            _loginRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));
        }
    }
}
