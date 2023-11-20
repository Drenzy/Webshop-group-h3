namespace WebshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        //Getting the interface and setting up as a private variable, so we can use it in other methodes
        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllASync()
        {
            //We use a try methode to get an answer if anything goes wrong,
            //we can print a message and not let the user completle blind over the problem
            try
            {
                //We are using the GetAllAsync methode from the Interface and set it into a var
                var categorys = await _categoryRepository.GetAllAsync();

                //We are selecting and mapping the categorys we got from the database and making it into a list of category responses
                List<CategoryResponse> categoryResponses = categorys.Select(
                    category => MapCategoryToCategoryResponse(category)).ToList();

                return Ok(categoryResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CategoryRequest categoryRequest)
        {
            try
            {
                Category newCategory = MapCategoryRequestToCategory(categoryRequest);

                var category = await _categoryRepository.CreateAsync(newCategory);

                CategoryResponse categoryResponse = MapCategoryToCategoryResponse(category);

                return Ok(categoryResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin, Role.User)]
        [HttpGet]
        [Route("{categoryId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int categoryId)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(categoryId);
                if (category == null)
                {
                    return NotFound();
                }

                return Ok(MapCategoryToCategoryResponse(category));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPut]
        [Route("{categoryId}")]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] int categoryId, [FromBody] CategoryRequest categoryRequest)
        {
            try
            {
                var updatecategory = MapCategoryRequestToCategory(categoryRequest);

                var category = await _categoryRepository.UpdateByIdAsync(categoryId, updatecategory);

                if (category == null)
                {
                    return NotFound();
                }

                return Ok(MapCategoryToCategoryResponse(category));
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpDelete]
        [Route("{categoryId}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] int categoryId)
        {
            try
            {
                var category = await _categoryRepository.DeleteByIdAsync(categoryId);
                if (category == null)
                {
                    return NotFound();
                }

                return Ok(MapCategoryToCategoryResponse(category));
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        private CategoryResponse MapCategoryToCategoryResponse(Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        private Category MapCategoryRequestToCategory(CategoryRequest categoryRequest)
        {
            return new Category
            {
                Name = categoryRequest.Name
            };
        }
    }
}
