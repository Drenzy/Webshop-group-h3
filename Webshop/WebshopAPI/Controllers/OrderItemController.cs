namespace WebshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;

        //Getting the interface and setting up as a private variable, so we can use it in other methodes
        public OrderItemController(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] OrderItemRequest orderItemRequest)
        {
            try
            {
                OrderItem newOrderItem = MapOrderItemRequestToOrderItem(orderItemRequest);

                var orderItem = await _orderItemRepository.CreateAsync(newOrderItem);

                OrderItemResponse orderItemResponse = MapOrderItemToOrderItemResponse(orderItem);

                return Ok(orderItemResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        [Route("{orderItemId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int orderItemId)
        {
            try
            {
                var orderItem = await _orderItemRepository.FindByIdAsync(orderItemId);
                if (orderItem == null)
                {
                    return NotFound();
                }

                return Ok(MapOrderItemToOrderItemResponse(orderItem));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPut]
        [Route("{orderItemId}")]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] int orderItemId, [FromBody] OrderItemRequest orderItemRequest)
        {
            try
            {
                var updateorderItem = MapOrderItemRequestToOrderItem(orderItemRequest);

                var orderItem = await _orderItemRepository.UpdateByIdAsync(orderItemId, updateorderItem);

                if (orderItem == null)
                {
                    return NotFound();
                }

                return Ok(MapOrderItemToOrderItemResponse(orderItem));
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpDelete]
        [Route("{orderItemId}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] int orderItemId)
        {
            try
            {
                var orderItem = await _orderItemRepository.DeleteByIdAsync(orderItemId);
                if (orderItem == null)
                {
                    return NotFound();
                }

                return Ok(MapOrderItemToOrderItemResponse(orderItem));
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        private OrderItemResponse MapOrderItemToOrderItemResponse(OrderItem orderItem)
        {
            OrderItemResponse response = new OrderItemResponse
            {
                Id = orderItem.Id,
                Price = orderItem.Price,
                Quantity = orderItem.Quantity,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
            };
            if (orderItem.Product != null)
            {
                response.Name = orderItem.Product.Name;
            };
            return response;
        }

        private OrderItem MapOrderItemRequestToOrderItem(OrderItemRequest orderItemRequest)
        {
            return new OrderItem
            {
                Price = orderItemRequest.Price,
                Quantity = orderItemRequest.Quantity,
                OrderId = orderItemRequest.OrderId,
                ProductId = orderItemRequest.ProductId,
            };
        }
    }
}
