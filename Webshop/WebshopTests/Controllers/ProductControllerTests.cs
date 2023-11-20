namespace WebshopTests.Controllers
{
    public class ProductControllerTests
    {
        private readonly ProductController _productController;
        private readonly Mock<IProductRepository> _productRepositoryMock = new();
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock = new();
        public ProductControllerTests()
        {
            _productController = new(_productRepositoryMock.Object, _categoryRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenProductsExists()
        {
            //Arrange
            List<Product> products = new()
            {
                new Product
                {
                    Id = 1,
                    Name = "Blå Sko",
                    Description = "Den blå sko",
                    Price = 200
                },
                new Product
                {
                    Id = 2,
                    Name = "Sort Jeans",
                    Description = "De sorte Wrangler",
                    Price = 199.99M
                }
            };

            _productRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(products);

            //Act
            var result = await _productController.GetAllASync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<ProductResponse>>(objectResult.Value);
            var data = objectResult.Value as List<ProductResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            _productRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _productController.GetAllASync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<ProductResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_WhenProductIsSuccessfullyCreated()
        {
            //Arrange
            int categoryId = 1;
            Category category = new()
            {
                Id = categoryId,
                Name = "Det grå"
            };
            _categoryRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(category);

            ProductRequest productRequest = new()
            {
                Name = "Blå Sko",
                Description = "Den blå sko",
                Price = 200,
                CategoryId = 1
            };

            int productId = 1;
            Product product = new()
            {
                Id = productId,
                Name = "Blå Sko",
                Description = "Den blå sko",
                Price = 200,
                CategoryId = 1
            };
            _productRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Product>()))
                .ReturnsAsync(product);

            //Act
            var result = await _productController.CreateAsync(productRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ProductResponse;
            Assert.NotNull(data);
            Assert.Equal(productId, data.Id);
            Assert.Equal(product.Name, data.Name);
            Assert.Equal(product.Description, data.Description);
            Assert.Equal(product.Price, data.Price);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode404_WhenCategoryIsNotFound()
        {
            //Arrange
            int categoryId = 1;

            _categoryRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            ProductRequest productRequest = new()
            {
                Name = "Blå Sko",
                Description = "Den blå sko",
                Price = 200,
                CategoryId = 1
            };

            int productId = 1;
            Product product = new()
            {
                Id = productId,
                Name = "Blå Sko",
                Description = "Den blå sko",
                Price = 200,
                CategoryId = 1
            };
            _productRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Product>()))
                .ReturnsAsync(product);

            //Act
            var result = await _productController.CreateAsync(productRequest);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int categoryId = 1;
            Category category = new()
            {
                Id = categoryId,
                Name = "Det grå"
            };
            _categoryRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(category);

            ProductRequest productRequest = new()
            {
                Name = "Blå Sko",
                Description = "Den blå sko",
                Price = 200
            };


            _productRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Product>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _productController.CreateAsync(productRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenProductExists()
        {
            //Arrange
            int productId = 1;
            ProductResponse productResponse = new()
            {
                Id = productId,
                Name = "Blå Sko",
                Description = "Den blå sko",
                Price = 200
            };
            Product product = new()
            {
                Id = productId,
                Name = "Blå Sko",
                Description = "Den blå sko",
                Price = 200
            };
            _productRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);

            //Act
            var result = await _productController.FindByIdAsync(productId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ProductResponse;
            Assert.NotNull(data);
            Assert.Equal(productId, data.Id);
            Assert.Equal(product.Name, data.Name);
            Assert.Equal(product.Description, data.Description);
            Assert.Equal(product.Price, data.Price);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenProductDoesNotExist()
        {
            //Arrange
            int productId = 1;

            _productRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _productController.FindByIdAsync(productId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int productId = 1;

            _productRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _productController.FindByIdAsync(productId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenProductIsUpdated()
        {
            //Arrange
            ProductRequest productRequest = new()
            {
                Name = "Blå Sko",
                Description = "Den blå sko",
                Price = 200
            };

            int productId = 1;
            Product product = new()
            {
                Id = productId,
                Name = "Blå Sko",
                Description = "Den blå sko",
                Price = 200
            };
            _productRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Product>()))
                .ReturnsAsync(product);

            //Act
            var result = await _productController.UpdateByIdAsync(productId, productRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ProductResponse;
            Assert.NotNull(data);
            Assert.Equal(productRequest.Name, data.Name);
            Assert.Equal(productRequest.Description, data.Description);
            Assert.Equal(productRequest.Price, data.Price);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenProductDoesNotExist()
        {
            //Arrange
            ProductRequest productRequest = new()
            {
                Name = "Blå Sko",
                Description = "Den blå sko",
                Price = 200
            };

            int productId = 1;
            _productRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Product>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _productController.UpdateByIdAsync(productId, productRequest);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            ProductRequest productRequest = new()
            {
                Name = "Blå Sko",
                Description = "Den blå sko",
                Price = 200
            };

            int productId = 1;
            _productRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Product>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _productController.UpdateByIdAsync(productId, productRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenProductIsDeleted()
        {
            //Arrange
            int productId = 1;

            Product product = new()
            {
                Id = productId,
                Name = "Blå Sko",
                Description = "Den blå sko",
                Price = 200
            };
            _productRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(product);

            //Act
            var result = await _productController.DeleteByIdAsync(productId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as ProductResponse;
            Assert.NotNull(data);
            Assert.Equal(productId, data.Id);
            Assert.Equal(product.Name, data.Name);
            Assert.Equal(product.Description, data.Description);
            Assert.Equal(product.Price, data.Price);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenProductDoesNotExist()
        {
            //Arrange
            int productId = 1;

            _productRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _productController.DeleteByIdAsync(productId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int productId = 1;
            _productRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _productController.DeleteByIdAsync(productId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
