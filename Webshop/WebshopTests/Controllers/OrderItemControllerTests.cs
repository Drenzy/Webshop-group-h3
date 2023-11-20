namespace WebshopTests.Controllers
{
    public class OrderItemControllerTests
    {
        private readonly OrderItemController _orderItemController;
        private readonly Mock<IOrderItemRepository> _orderItemRepositoryMock = new();
        public OrderItemControllerTests()
        {
            _orderItemController = new(_orderItemRepositoryMock.Object);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_WhenorderItemIsSuccessfullyCreated()
        {
            //Arrange
            OrderItemRequest orderItemRequest = new()
            {
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1,
            };

            int orderItemId = 1;

            OrderItem orderItem = new()
            {
                Id = orderItemId,
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1,
            };
            _orderItemRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<OrderItem>()))
                .ReturnsAsync(orderItem);

            //Act
            var result = await _orderItemController.CreateAsync(orderItemRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as OrderItemResponse;
            Assert.NotNull(data);
            Assert.Equal(orderItemId, data.Id);
            Assert.Equal(orderItem.Price, data.Price);
            Assert.Equal(orderItem.Quantity, data.Quantity);
            Assert.Equal(orderItem.OrderId, data.OrderId);
            Assert.Equal(orderItem.ProductId, data.ProductId);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            OrderItemRequest orderItemRequest = new()
            {
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1,
            };


            _orderItemRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<OrderItem>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _orderItemController.CreateAsync(orderItemRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }


        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenOrderItemExists()
        {
            //Arrange
            int orderItemId = 1;
            OrderItemResponse orderItemResponse = new()
            {
                Id = orderItemId,
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1
            };
            OrderItem orderItem = new()
            {
                Id = orderItemId,
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1
            };
            _orderItemRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(orderItem);

            //Act
            var result = await _orderItemController.FindByIdAsync(orderItemId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as OrderItemResponse;
            Assert.NotNull(data);
            Assert.Equal(orderItemId, data.Id);
            Assert.Equal(orderItem.Price, data.Price);
            Assert.Equal(orderItem.Quantity, data.Quantity);
            Assert.Equal(orderItem.OrderId, data.OrderId);
            Assert.Equal(orderItem.ProductId, data.ProductId);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenOrderItemDoesNotExist()
        {
            //Arrange
            int orderItemId = 1;

            _orderItemRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _orderItemController.FindByIdAsync(orderItemId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int orderItemId = 1;

            _orderItemRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _orderItemController.FindByIdAsync(orderItemId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenOrderItemIsUpdated()
        {
            //Arrange
            OrderItemRequest orderItemRequest = new()
            {
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1,
            };

            int orderItemId = 1;
            OrderItem orderItem = new()
            {
                Id = orderItemId,
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1,
            };
            _orderItemRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<OrderItem>()))
                .ReturnsAsync(orderItem);

            //Act
            var result = await _orderItemController.UpdateByIdAsync(orderItemId, orderItemRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as OrderItemResponse;
            Assert.NotNull(data);
            Assert.Equal(orderItem.Price, data.Price);
            Assert.Equal(orderItem.Quantity, data.Quantity);
            Assert.Equal(orderItem.OrderId, data.OrderId);
            Assert.Equal(orderItem.ProductId, data.ProductId);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenOrderItemDoesNotExist()
        {
            //Arrange
            OrderItemRequest orderItemRequest = new()
            {
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1,
            };

            int orderItemId = 1;
            _orderItemRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<OrderItem>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _orderItemController.UpdateByIdAsync(orderItemId, orderItemRequest);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            OrderItemRequest orderItemRequest = new()
            {
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1,
            };

            int orderItemId = 1;
            _orderItemRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<OrderItem>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _orderItemController.UpdateByIdAsync(orderItemId, orderItemRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode200_WhenOrderItemIsDeleted()
        {
            //Arrange
            int orderItemId = 1;

            OrderItem orderItem = new()
            {
                Id = orderItemId,
                Price = 20,
                Quantity = 1,
                OrderId = 1,
                ProductId = 1,
            };
            _orderItemRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(orderItem);

            //Act
            var result = await _orderItemController.DeleteByIdAsync(orderItemId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as OrderItemResponse;
            Assert.NotNull(data);
            Assert.Equal(orderItemId, data.Id);
            Assert.Equal(orderItem.Price, data.Price);
            Assert.Equal(orderItem.Quantity, data.Quantity);
            Assert.Equal(orderItem.OrderId, data.OrderId);
            Assert.Equal(orderItem.ProductId, data.ProductId);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode404_WhenOrderItemDoesNotExist()
        {
            //Arrange
            int orderItemId = 1;

            _orderItemRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _orderItemController.DeleteByIdAsync(orderItemId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void DeleteByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int orderItemId = 1;
            _orderItemRepositoryMock
                .Setup(x => x.DeleteByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _orderItemController.DeleteByIdAsync(orderItemId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
