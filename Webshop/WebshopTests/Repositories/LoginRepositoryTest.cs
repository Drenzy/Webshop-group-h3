namespace WebshopTests.Repositories
{
    public class LoginRepositoryTest
    {
        private readonly DbContextOptions<DatabaseContext> _options;
        private readonly DatabaseContext _context;
        private readonly LoginRepository _loginRepository;

        public LoginRepositoryTest()
        {
            _options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "LoginRepositoryTests")
                .Options;

            _context = new(_options);

            _loginRepository = new(_context);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnListOfLogins_WhenApparelExists()
        {
            //Arrange
            // Makes sure the database is deleted
            await _context.Database.EnsureDeletedAsync();

            // Adding data into context which is a "copy" of the database
            _context.Login.Add(new Login
            {
                Id = 1,
                UserName= "Test",
                Password = "Test",
                Email= "Test",
                Role= Role.Admin
            });
            _context.Login.Add(new Login
            {
                Id = 2,
                UserName = "Test",
                Password = "Test",
                Email = "Test",
                Role = Role.Admin
            });

            // Saves the added changes 
            await _context.SaveChangesAsync();

            // Act
            // Waiting for the task to be completed
            var result = await _loginRepository.GetAllAsync();

            //assert
            // Makes sure it is NotNUll
            Assert.NotNull(result);
            // Makes sure its the right type it is given
            Assert.IsType<List<Login>>(result);
            // Verifying that the two the objects are eaquel / making sure there are 2 entries in the database
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnEmptyListOfLogins_WhenNoLoginExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();


            // Act
            var result = await _loginRepository.GetAllAsync();

            //assert
            Assert.NotNull(result);
            Assert.IsType<List<Login>>(result);
            // Veryfies that the collection is empty
            Assert.Empty(result);
        }

        [Fact]
        public async void CreateAsync_ShouldAddNewIdToLogin_WhenSavingToDatabase()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int expectedId = 1;

            Login login = new()
            {
                Id = 1,
                UserName = "Test",
                Password = "Test",
                Email = "Test",
                Role = Role.Admin
            };
            //Act
            var result = await _loginRepository.CreateAsync(login);
            //Assert
            Assert.NotNull(result);
            Assert.IsType<Login>(result);
            // Verifying that the expected ID is the same as the result ID.
            Assert.Equal(expectedId, result?.Id);
        }

        [Fact]
        public async void CreateAsync_ShouldFailToAddNewLogin_WhenLoginIdAlreadyExists()
        {
            await _context.Database.EnsureDeletedAsync();


            Login login = new()
            {
                Id = 1,
                UserName = "Test",
                Password = "Test",
                Email = "Test",
                Role = Role.Admin
            };
            await _loginRepository.CreateAsync(login);
            //Act
            async Task action() => await _loginRepository.CreateAsync(login);
            //Assert
            // Verifying that the error message is outputed
            var ex = await Assert.ThrowsAsync<ArgumentException>(action);
            Assert.Contains("An item with the same key has already been added", ex.Message);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnLogin_WhenLoginExists()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int loginId = 1;

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
                Id = loginId,
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

            await _context.SaveChangesAsync();

            //Act
            var reusult = await _loginRepository.FindByIdAsync(loginId);

            //Assert
            Assert.NotNull(reusult);
            
            Assert.Equal(loginId, reusult.Id);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnNull_WhenLoginDoesNotExist()
        {
            //Arrange
            await _context.Database.EnsureDeletedAsync();

            int loginId = 1;

            //Act
            var reusult = await _loginRepository.FindByIdAsync(loginId);

            // Assert
            Assert.Null(reusult);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnUpdatedLogin_WhenLoginExists()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int loginId = 1;

            _context.Login.Add(new()
            {
                Id = loginId,
                UserName = "Test",
                Password = "Test",
                Email = "Test",
                Role = Role.Admin
            });

            await _context.SaveChangesAsync();

            Login updateLogin = new()
            {
                UserName = "Test2",
                Password = "Test2",
                Email = "Test2",
                Role = Role.User
            };

            var result = await _loginRepository.UpdateByIdAsync(loginId, updateLogin);

            Assert.NotNull(result);
            Assert.IsType<Login>(result);
            Assert.Equal(updateLogin.UserName, result.UserName);
            Assert.Equal(updateLogin.Password, result.Password);
            Assert.Equal(updateLogin.Email, result.Email);
            Assert.Equal(updateLogin.Role, result.Role);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnNull_WhenLoginDoesNotExist()
        {
            // Arrange
            await _context.Database.EnsureDeletedAsync();

            int loginId = 1;

            Login updateLogin = new()
            {
                UserName = "Test",
                Password = "Test",
                Email = "Test",
                Role = Role.Admin
            };
            var result = await _loginRepository.UpdateByIdAsync(loginId, updateLogin);

            Assert.Null(result);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnLogin_WhenLoginIsDeleted()
        {
            await _context.Database.EnsureDeletedAsync();

            int loginId = 1;

            Login login = new()
            {
                UserName = "Test",
                Password = "Test",
                Email = "Test",
                Role = Role.Admin
            };

            await _loginRepository.CreateAsync(login);

            var result = await _loginRepository.DeleteByIdAsync(loginId);

            Assert.NotNull(result);
            Assert.IsType<Login>(result);
            Assert.Equal(loginId, result.Id);
            Assert.Equal(login.UserName, result.UserName);
            Assert.Equal(login.Password, result.Password);
            Assert.Equal(login.Email, result.Email);
            Assert.Equal(login.Role, result.Role);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnNull_WhenLoginDoesNotExist()
        {
            await _context.Database.EnsureDeletedAsync();

            int loginId = 1;

            var result = await _loginRepository.DeleteByIdAsync(loginId);

            Assert.Null(result);
        }
    }
}
