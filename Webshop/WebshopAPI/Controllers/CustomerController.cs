namespace WebshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        private static CustomerResponse MapCustomerToCustomerResponse(Customer customer)
        {
            CustomerResponse response = new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                AddressId = customer.AddressId,
                PhoneNr = customer.PhoneNr,
                LoginId = customer.LoginId
            }; 
            if(customer.Login != null)
            {
                response.Logins = new CustomerLoginResponse
                {
                    Id = customer.Login.Id,
                    UserName = customer.Login.UserName,
                    Email = customer.Login.Email
                };
            }
            if(customer.Address != null)
            {
                response.Addresses = new CustomerAddressResponse
                {
                    StreetName = customer.Address.StreetName,
                    ZipCodeId = customer.Address.ZipCodeId,
                    Country = customer.Address.Country,
                    City = customer.Address.ZipCode.City
                };
            }
            return response;
        }

        private static Customer MapCustomerRequestToCustomer(CustomerRequest customerRequest)
        {
            return new()
            {
                Name = customerRequest.Name,
                AddressId = customerRequest.AddressId,
                PhoneNr = customerRequest.PhoneNr,
                LoginId = customerRequest.LoginId
            };
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var customers = await _customerRepository.GetAllAsync();
                List<CustomerResponse> customerResponse = customers.Select(
                    customer => MapCustomerToCustomerResponse(customer)).ToList();

                return Ok(customerResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CustomerRequest customerRequest)
        {
            try
            {
                Customer newCustomer = MapCustomerRequestToCustomer(customerRequest);

                var login = await _customerRepository.CreateAsync(newCustomer);
                CustomerResponse customerResponse = MapCustomerToCustomerResponse(login);

                return Ok(customerResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        [Route("{customerId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int customerId)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(customerId);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(MapCustomerToCustomerResponse(customer));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPut]
        [Route("{customerId}")]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] int customerId, [FromBody] CustomerRequest customerRequest)
        {
            try
            {
                var updateCustomer = MapCustomerRequestToCustomer(customerRequest);

                var customer = await _customerRepository.UpdateByIdAsync(customerId, updateCustomer);

                if (customer == null)
                {
                    return NotFound();
                }

                return Ok(MapCustomerToCustomerResponse(customer));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpDelete]
        [Route("{customerId}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] int customerId)
        {
            try
            {
                var login = await _customerRepository.DeleteByIdAsync(customerId);
                if (login == null)
                {
                    return NotFound();
                }
                return Ok(MapCustomerToCustomerResponse(login));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
