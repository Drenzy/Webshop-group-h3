namespace WebshopTests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly CategoryController _categoryController;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();
        public CategoryControllerTests()
        {
            _categoryController = new(_categoryRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenCategorysExists()
        {
            //Arrange
            List<Category> categorys = new()
            {
                new Category
                {
                    Id = 1,
                    Name = "Sko"
                },
                new Category
                {
                    Id = 2,
                    Name = "Bukser"
                }
            };

            _categoryRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(categorys);

            //Act
            var result = await _categoryController.GetAllASync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<CategoryResponse>>(objectResult.Value);
            var data = objectResult.Value as List<CategoryResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            _categoryRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _categoryController.GetAllASync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<CategoryResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_WhencategoryIsSuccessfullyCreated()
        {
            //Arrange
            CategoryRequest categoryRequest = new()
            {
                Name = "Sko"
            };

            int categoryId = 1;
            Category category = new()
            {
                Id = categoryId,
                Name = "Sko"
            };
            _categoryRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Category>()))
                .ReturnsAsync(category);

            //Act
            var result = await _categoryController.CreateAsync(categoryRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as CategoryResponse;
            Assert.NotNull(data);
            Assert.Equal(categoryId, data.Id);
            Assert.Equal(category.Name, data.Name);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            CategoryRequest categoryRequest = new()
            {
                Name = "Sko"
            };


            _categoryRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Category>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _categoryController.CreateAsync(categoryRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }


        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenCategoryExists()
        {
            //Arrange
            int categoryId = 1;
            CategoryResponse categoryResponse = new()
            {
                Id = categoryId,
                Name = "Sko"
            };
            Category category = new()
            {
                Id = categoryId,
                Name = "Sko"
            };
            _categoryRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(category);

            //Act
            var result = await _categoryController.FindByIdAsync(categoryId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as CategoryResponse;
            Assert.NotNull(data);
            Assert.Equal(categoryId, data.Id);
            Assert.Equal(category.Name, data.Name);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenCategoryDoesNotExist()
        {
            //Arrange
            int categoryId = 1;

            _categoryRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _categoryController.FindByIdAsync(categoryId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int categoryId = 1;

            _categoryRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _categoryController.FindByIdAsync(categoryId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenCategoryIsUpdated()
        {
            //Arrange
            CategoryRequest categoryRequest = new()
            {
                Name = "Sko"
            };

            int categoryId = 1;
            Category category = new()
            {
                Id = categoryId,
                Name = "Sko"
            };
            _categoryRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Category>()))
                .ReturnsAsync(category);

            //Act
            var result = await _categoryController.UpdateByIdAsync(categoryId, categoryRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as CategoryResponse;
            Assert.NotNull(data);
            Assert.Equal(categoryRequest.Name, data.Name);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenCategoryDoesNotExist()
        {
            //Arrange
            CategoryRequest categoryRequest = new()
            {
                Name = "Sko"
            };

            int categoryId = 1;
            _categoryRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Category>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _categoryController.UpdateByIdAsync(categoryId, categoryRequest);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            CategoryRequest categoryRequest = new()
            {
                Name = "Sko"
            };

            int categoryId = 1;
            _categoryRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Category>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _categoryController.UpdateByIdAsync(categoryId, categoryRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenCategoryIsDeleted()
        {
            //Arrange
            int categoryId = 1;

            Category category = new()
            {
                Id = categoryId,
                Name = "Sko"
            };
            _categoryRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(category);

            //Act
            var result = await _categoryController.DeleteByIdAsync(categoryId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as CategoryResponse;
            Assert.NotNull(data);
            Assert.Equal(categoryId, data.Id);
            Assert.Equal(category.Name, data.Name);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenCategoryDoesNotExist()
        {
            //Arrange
            int categoryId = 1;

            _categoryRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _categoryController.DeleteByIdAsync(categoryId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int categoryId = 1;
            _categoryRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _categoryController.DeleteByIdAsync(categoryId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
