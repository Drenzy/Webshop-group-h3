namespace WebshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipCodeController : ControllerBase
    {
        private readonly IZipCodeRepository _zipCodeRepository;

        public ZipCodeController(IZipCodeRepository zipCodeRepository)
        {
            _zipCodeRepository = zipCodeRepository;
        }
        private static ZipCodeResponse MapZipCodeToZipCodeResponse(ZipCode zipcode)
        {
            ZipCodeResponse response = new ZipCodeResponse
            {
                Id = zipcode.Id,
                City = zipcode.City
            };
            return response;
        }

        [HttpGet]
        [Route("{zipcodeId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int zipcodeId)
        {
            try
            {
                var zipcode = await _zipCodeRepository.FindByIdAsync(zipcodeId);
                if (zipcode == null)
                {
                    return NotFound();
                }
                return Ok(
                    new ZipCodeResponse
                    {
                        Id = zipcode.Id,
                        City = zipcode.City
                    });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var zipcodes = await _zipCodeRepository.GetAllAsync();
                List<ZipCodeResponse> zipcodeResponse = zipcodes.Select(
                    zipcode => MapZipCodeToZipCodeResponse(zipcode)).ToList();

                return Ok(zipcodeResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
