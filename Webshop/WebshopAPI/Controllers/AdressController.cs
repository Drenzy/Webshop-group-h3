namespace WebshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _adressRepository;

        public AddressController(IAddressRepository adressRepository)
        {
            //assigning our private variable to our controller
            _adressRepository = adressRepository;
        }

        private static AddressResponse MapAddressToAddressResponse(Address adress)
        {
            //LAVE OM
            AddressResponse response = new AddressResponse
            {
                Id= adress.Id,
                StreetName= adress.StreetName,
                ZipCodeId = adress.ZipCodeId,
                Country= adress.Country,
            };
            if (adress.ZipCode != null)
            {
                response.City = adress.ZipCode.City;
            };
            return response;
        }

        // request for address which is the input given to the controller for the coloumns 
        private static Address MapAddressRequestToAddress(AddressRequest addressRequest)
        {
            Address address = new Address
            {
                StreetName = addressRequest.StreetName,
                ZipCodeId = addressRequest.ZipCodeId,
                Country = addressRequest.Country
            };
            return address;
        }

        // only loings with the role admin can access this endpoint
        [Authorize(Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                // using our GetAll method to save our addresses into our address var
                var adresses = await _adressRepository.GetAllAsync();
                // making a list of addresses from our database
                List<AddressResponse> adressResponse = adresses.Select(
                    // using our mappingToResponseMethod and adding it to a list
                    adress => MapAddressToAddressResponse(adress)).ToList();

                // returns an OkRepsone which is a 200 repsonse
                return Ok(adressResponse);
            }
            // catch Exception
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpGet]
        // specid endpoint URL
        [Route("{addressId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int addressId)
        {
            try
            {
                // using our FindById from our Repository
                var address = await _adressRepository.FindByIdAsync(addressId);
                if (address == null)
                {
                    // if address is null return nothing
                    return NotFound();
                }
                // if address is was foudn return Ok address
                return Ok(MapAddressToAddressResponse(address));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] AddressRequest addressRequest)
        {
            try
            {
                // using our request method to take in inputs
                Address newAddress = MapAddressRequestToAddress(addressRequest);

                // calling our Create method from our Repository
                var address = await _adressRepository.CreateAsync(newAddress);
                
                // 
                AddressResponse addressResponse = MapAddressToAddressResponse(address);

                return Ok(addressResponse);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPut]
        [Route("{addressId}")]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] int addressId, [FromBody] AddressRequest addressRequest)
        {
            try
            {
                var updateAddress = MapAddressRequestToAddress(addressRequest);

                var address = await _adressRepository.UpdateByIdAsync(addressId, updateAddress);

                if(address == null)
                {
                    return NotFound();
                }

                return Ok(MapAddressToAddressResponse(address));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
