namespace WebshopTests.Controllers
{

    public class OrderControllerTests
    {
        private readonly OrderController _orderController;
        private readonly Mock<IOrderRepository> _orderRepositoryMock = new();
        public OrderControllerTests()
        {
            _orderController = new(_orderRepositoryMock.Object);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode200_WhenOrdersExists()
        {
            //Arrange
            List<Order> orders = new()
            {
                new Order
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Parse("2002-01-12"),
                    StatusId = 4
                },
                new Order
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = DateTime.Parse("2023-01-12"),
                    StatusId = 1
                }
            };

            _orderRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(orders);

            //Act
            var result = await _orderController.GetAllASync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<OrderResponse>>(objectResult.Value);
            var data = objectResult.Value as List<OrderResponse>;
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
        }

        [Fact]
        public async void GetAllAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            _orderRepositoryMock
                .Setup(a => a.GetAllAsync())
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _orderController.GetAllASync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<OrderResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void GetAllByStatusIdAsync_ShouldReturnStatusCode200_WhenOrdersExists()
        {
            //Arrange
            int statusId = 3;

            List<Order> orders = new()
            {
                new Order
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = DateTime.Parse("2002-01-12"),
                    StatusId = 3
                },
                new Order
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = DateTime.Parse("2023-01-12"),
                    StatusId = 3
                },
                new Order
                {
                    Id = 3,
                    CustomerId = 1,
                    OrderDate = DateTime.Parse("2023-01-12"),
                    StatusId = 4
                }
            };

            _orderRepositoryMock
                .Setup(a => a.GetAllByStatusIdAsync(statusId))
                .ReturnsAsync(orders);

            //Act
            var result = await _orderController.GetAllByStatusIdAsync(statusId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            Assert.NotNull(objectResult.Value);
            Assert.IsType<List<OrderResponse>>(objectResult.Value);
            var data = objectResult.Value as List<OrderResponse>;
            Assert.NotNull(data);
            Assert.Equal(3, data.Count);
        }

        [Fact]
        public async void GetAllByStatusIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            int statusId = 1;
            //Arrange
            _orderRepositoryMock
                .Setup(a => a.GetAllByStatusIdAsync(statusId))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _orderController.GetAllASync();

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);

            var data = objectResult.Value as List<OrderResponse>;
            Assert.Null(data);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode200_WhenorderIsSuccessfullyCreatedWithAnCustomerIdAndAnAddressId()
        {
            //Arrange
            OrderCreateRequest orderRequest = new()
            {
                CustomerId = 1,
                AddressId= 1,
                OrderItems = new()
            };
            OrderCreateRequestOrderItem orderItemRequest = new()
            {
                ProductId = 1,
                Quantity = 1,
                Price = 200
            };

            orderRequest.OrderItems.Add(orderItemRequest);

            int orderId = 1;
            Order order = new()
            {
                Id = orderId,
                CustomerId = 1,
                OrderDate = DateTime.Now,
                StatusId = 1,
                AddressId = 1,
                OrderItems = new()
            };
            OrderItem orderItem = new()
            {
                Id = 1,
                ProductId = 1,
                Quantity = 1,
                Price = 200,
                OrderId = orderId,
            };
            order.OrderItems.Add(orderItem);

            _orderRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Order>()))
                .ReturnsAsync(order);

            //Act
            var result = await _orderController.CreateAsync(orderRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as OrderResponse;
            Assert.NotNull(data);
            Assert.Equal(orderId, data.Id);
            Assert.Equal(order.CustomerId, data.CustomerId);
            Assert.Equal(order.OrderDate, data.OrderDate);
            Assert.Equal(order.StatusId, data.StatusId);
            Assert.Equal(order.AddressId, data.AddressId);
            Assert.IsType<List<OrderOrderItemResponse>>(data.OrderItems);
            Assert.Equal(1, data.OrderItems.Count);
            Assert.Equal(orderItem.Quantity, data.OrderItems[0].Quantity);
        }

        [Fact]

        public async void CreateAsync_ShouldReturnStatusCode200_WhenorderIsSuccessfullyCreatedWithANewAddress()
        {
            //Arrange
            OrderCreateRequest orderRequest = new()
            {
                CustomerId = 1,
                AddressId = 0,
                OrderItems = new()
            };
            OrderCreateRequestOrderItem orderItemRequest = new()
            {
                ProductId = 1,
                Quantity = 1,
                Price = 200
            };
            OrderCreateAddressRequest AddressRequest = new()
            {
                StreetName = "Telegrafvej 9",
                ZipCodeId = 2300,
                Country = "Danmark"
            };

            orderRequest.OrderItems.Add(orderItemRequest);
            orderRequest.Address = AddressRequest;

            int orderId = 1;
            Order order = new()
            {
                Id = orderId,
                CustomerId = 1,
                OrderDate = DateTime.Now,
                StatusId = 1,
                OrderItems = new()
            };
            OrderItem orderItem = new()
            {
                Id = 1,
                ProductId = 1,
                Quantity = 1,
                Price = 200,
                OrderId = orderId,
            };
            Address Address = new()
            {
                Id = 1,
                StreetName = "Telegrafvej 9",
                ZipCodeId = 2300,
                Country = "Danmark"
            };
            order.OrderItems.Add(orderItem);
            order.Address = Address;

            _orderRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Order>()))
                .ReturnsAsync(order);

            //Act
            var result = await _orderController.CreateAsync(orderRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as OrderResponse;
            Assert.NotNull(data);
            Assert.Equal(orderId, data.Id);
            Assert.Equal(order.CustomerId, data.CustomerId);
            Assert.Equal(order.OrderDate, data.OrderDate);
            Assert.Equal(order.StatusId, data.StatusId);
            Assert.Equal(0, data.AddressId);
            Assert.Equal(order.Address.StreetName, data.Address.StreetName);
            Assert.Equal(order.Address.ZipCodeId, data.Address.ZipCodeId);
            Assert.Equal(order.Address.Country, data.Address.Country);
            Assert.IsType<List<OrderOrderItemResponse>>(data.OrderItems);
            Assert.Equal(1, data.OrderItems.Count);
            Assert.Equal(orderItem.Quantity, data.OrderItems[0].Quantity);
        }

        [Fact]
        public async void CreateAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            OrderCreateRequest orderRequest = new()
            {
                CustomerId = 1,
                AddressId = 1
            };


            _orderRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Order>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _orderController.CreateAsync(orderRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode200_WhenOrderExists()
        {
            //Arrange
            int orderId = 1;
            OrderResponse orderResponse = new()
            {
                Id = orderId,
                CustomerId = 1,
                OrderDate = DateTime.Parse("2002-01-12"),
                StatusId = 4,
                AddressId = 1
            };
            Order order = new()
            {
                Id = orderId,
                CustomerId = 1,
                OrderDate = DateTime.Parse("2002-01-12"),
                StatusId = 4,
                AddressId = 1
            };
            _orderRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(order);

            //Act
            var result = await _orderController.FindByIdAsync(orderId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as OrderResponse;
            Assert.NotNull(data);
            Assert.Equal(orderId, data.Id);
            Assert.Equal(order.CustomerId, data.CustomerId);
            Assert.Equal(order.OrderDate, data.OrderDate);
            Assert.Equal(order.StatusId, data.StatusId);
            Assert.Equal(order.AddressId, data.AddressId);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode404_WhenOrderDoesNotExist()
        {
            //Arrange
            int orderId = 1;

            _orderRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _orderController.FindByIdAsync(orderId);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void FindByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int orderId = 1;

            _orderRepositoryMock
                .Setup(x => x.FindByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _orderController.FindByIdAsync(orderId);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode200_WhenOrderIsUpdated()
        {
            //Arrange
            OrderRequest orderRequest = new()
            {
                CustomerId = 1,
                OrderDate = DateTime.Parse("2002-01-12"),
                StatusId = 4,
                AddressId = 2
            };

            int orderId = 1;
            Order order = new()
            {
                Id = orderId,
                CustomerId = 1,
                OrderDate = DateTime.Parse("2002-01-12"),
                StatusId = 4,
                AddressId = 1
            };
            _orderRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Order>()))
                .ReturnsAsync(order);

            //Act
            var result = await _orderController.UpdateByIdAsync(orderId, orderRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as OrderResponse;
            Assert.NotNull(data);
            Assert.Equal(order.CustomerId, data.CustomerId);
            Assert.Equal(order.OrderDate, data.OrderDate);
            Assert.Equal(order.StatusId, data.StatusId);
            Assert.Equal(order.AddressId, data.AddressId);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode404_WhenOrderDoesNotExist()
        {
            //Arrange
            OrderRequest orderRequest = new()
            {
                CustomerId = 1,
                OrderDate = DateTime.Parse("2002-01-12"),
                StatusId = 4,
                AddressId = 1
            };

            int orderId = 1;
            _orderRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Order>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _orderController.UpdateByIdAsync(orderId, orderRequest);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            OrderRequest orderRequest = new()
            {
                CustomerId = 1,
                OrderDate = DateTime.Parse("2002-01-12"),
                StatusId = 4,
                AddressId = 1
            };

            int orderId = 1;
            _orderRepositoryMock
                .Setup(x => x.UpdateByIdAsync(It.IsAny<int>(), It.IsAny<Order>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _orderController.UpdateByIdAsync(orderId, orderRequest);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateStatusByIdAsync_ShouldReturnStatusCode200_WhenOrderIsUpdated()
        {
            //Arrange
            int status = 1;

            int orderId = 1;
            Order order = new()
            {
                Id = orderId,
                CustomerId = 1,
                OrderDate = DateTime.Parse("2002-01-12"),
                StatusId = 4,
                AddressId = 1
            };
            _orderRepositoryMock
                .Setup(x => x.UpdateStatusByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(order);

            //Act
            var result = await _orderController.UpdateStatusByIdAsync(orderId, status);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(200, objectResult.StatusCode);

            var data = objectResult.Value as OrderResponse;
            Assert.NotNull(data);
            Assert.Equal(order.CustomerId, data.CustomerId);
            Assert.Equal(order.OrderDate, data.OrderDate);
            Assert.Equal(order.StatusId, data.StatusId);
            Assert.Equal(order.AddressId, data.AddressId);
        }

        [Fact]
        public async void UpdateStatusByIdAsync_ShouldReturnStatusCode404_WhenOrderDoesNotExist()
        {
            //Arrange
            int status = 1;

            int orderId = 1;
            _orderRepositoryMock
                .Setup(x => x.UpdateStatusByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //Act
            var result = await _orderController.UpdateStatusByIdAsync(orderId, status);

            //Assert
            var objectResult = result as NotFoundResult;
            Assert.NotNull(objectResult);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async void UpdateStatusByIdAsync_ShouldReturnStatusCode500_WhenExceptionIsRaised()
        {
            //Arrange
            int status = 1;

            int orderId = 1;
            _orderRepositoryMock
                .Setup(x => x.UpdateStatusByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => throw new Exception("This is an exception"));

            //Act
            var result = await _orderController.UpdateStatusByIdAsync(orderId, status);

            //Assert
            var objectResult = result as ObjectResult;
            Assert.NotNull(objectResult);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
