namespace WebshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        //Getting the interface and setting up as a private variable, so we can use it in other methodes
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllASync()
        {
            //We use a try methode to get an answer if anything goes wrong,
            //we can print a message and not let the user completle blind over the problem
            try
            {
                //We are using the GetAllAsync methode from the Interface and set it into a var
                var orders = await _orderRepository.GetAllAsync();

                //We are selecting and mapping the orders we got from the database and making it into a list of order responses
                List<OrderResponse> orderResponses = orders.Select(
                    order => MapOrderToOrderResponse(order)).ToList();

                return Ok(orderResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        [Route("status/{statusId}")]
        public async Task<IActionResult> GetAllByStatusIdAsync([FromRoute] int statusId)
        {
            //We use a try methode to get an answer if anything goes wrong,
            //we can print a message and not let the user completle blind over the problem
            try
            {
                //We are using the GetAllAsync methode from the Interface and set it into a var
                var orders = await _orderRepository.GetAllByStatusIdAsync(statusId);

                //We are selecting and mapping the orders we got from the database and making it into a list of order responses
                List<OrderResponse> orderResponses = orders.Select(
                    order => MapOrderToOrderResponse(order)).ToList();

                return Ok(orderResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        [Route("{orderId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int orderId)
        {
            try
            {
                var order = await _orderRepository.FindByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound();
                }

                return Ok(MapOrderToOrderResponse(order));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] OrderCreateRequest orderCreateRequest)
        {
            try
            {
                Order newOrder = new Order();
                newOrder.CustomerId = orderCreateRequest.CustomerId;
                newOrder.OrderDate = DateTime.Now;
                newOrder.StatusId = 1;
                newOrder.OrderItems = orderCreateRequest.OrderItems.Select(x => MapCarItemToOrderItem(x)).ToList();

                if (orderCreateRequest.AddressId != 0)
                {
                    newOrder.AddressId = orderCreateRequest.AddressId;
                }
                else
                {
                    newOrder.Address = new Address()
                    {
                        StreetName = orderCreateRequest.Address.StreetName,
                        Country = orderCreateRequest.Address.Country,
                        ZipCodeId = orderCreateRequest.Address.ZipCodeId,
                    };
                }

                var order = await _orderRepository.CreateAsync(newOrder);

                OrderResponse orderResponse = MapOrderToOrderResponse(order);

                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPut]
        [Route("{orderId}")]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] int orderId, [FromBody] OrderRequest orderRequest)
        {
            try
            {
                var updateorder = MapOrderRequestToOrder(orderRequest);

                var order = await _orderRepository.UpdateByIdAsync(orderId, updateorder);

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(MapOrderToOrderResponse(order));
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPatch]
        [Route("{orderId}/{statusId}")]
        public async Task<IActionResult> UpdateStatusByIdAsync([FromRoute] int orderId, [FromRoute] int statusId)
        {
            try
            {
                var order = await _orderRepository.UpdateStatusByIdAsync(orderId, statusId);

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(MapOrderToOrderResponse(order));
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        private OrderResponse MapOrderToOrderResponse(Order order)
        {
            OrderResponse response = new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                StatusId = order.StatusId,
                AddressId = order.AddressId
            };
            if(order.Customer != null)
            {
                response.Customer = new OrderCustomerResponse
                {
                    Id = order.Customer.Id,
                    Name = order.Customer.Name,
                    PhoneNr = order.Customer.PhoneNr,
                    Email = order.Customer.Login.Email
                };
            }
            if(order.Status != null)
            {
                response.Status = new OrderStatusResponse
                {
                    Id = order.Status.Id,
                    Name = order.Status.Name
                };
            }
            if(order.Address != null)
            {
                response.Address = new OrderAddressResponse
                {
                    Id = order.Address.Id,
                    StreetName = order.Address.StreetName,
                    ZipCodeId = order.Address.ZipCodeId,
                    Country = order.Address.Country,
                };
                if(order.Address.ZipCode != null)
                {
                    response.Address.City = order.Address.ZipCode.City;
                }
            }
            if(order.OrderItems != null)
            { 
                response.OrderItems = order.OrderItems.Select(orderItem => new OrderOrderItemResponse
                {
                    Id = orderItem.Id,
                    Price = orderItem.Price,
                    Quantity = orderItem.Quantity,
                    ProductId = orderItem.ProductId,
                    OrderId = order.Id,
                    Name = orderItem.Product?.Name?? ""
                }).ToList();

            }
            return response;
        }
        private Order MapOrderRequestToOrder(OrderRequest orderRequest)
        {
            return new Order
            {
                CustomerId = orderRequest.CustomerId,
                OrderDate = orderRequest.OrderDate,
                StatusId = orderRequest.StatusId,
                AddressId = orderRequest.AddressId,
            };
        }

        private OrderItem MapCarItemToOrderItem(OrderCreateRequestOrderItem orderitem)
        {
            return new OrderItem
            {
                Price = orderitem.Price,
                Quantity = orderitem.Quantity,
                ProductId = orderitem.ProductId,
            };
        }

    }
}
