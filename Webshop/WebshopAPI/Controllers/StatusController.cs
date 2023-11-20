namespace WebshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IStatusRepository _statusRepository;
        public StatusController(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        [Route("{statusId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int statusId)
        {
            try
            {
                var status = await _statusRepository.FindByIdAsync(statusId);
                if (status == null)
                {
                    return NotFound();
                }

                return Ok(MapStatusToStatusResponse(status));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var statuses = await _statusRepository.GetAllAsync();
                List<StatusResponse> statusResponse = statuses.Select(
                    status => MapStatusToStatusResponse(status)).ToList();

                return Ok(statusResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        private static StatusResponse MapStatusToStatusResponse(Status status)
        {
            StatusResponse response = new StatusResponse
            {
                Id = status.Id,
                Name = status.Name
            };
            return response;
        }
    }
}
