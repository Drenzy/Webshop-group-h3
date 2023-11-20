namespace WebshopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        //Getting the interface and setting up as a private variable, so we can use it in other methodes
        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
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
                var apprels = await _productRepository.GetAllAsync();

                //We are selecting and mapping the products we got from the database and making it into a list of product responses
                List<ProductResponse> productResponses = apprels.Select(
                    product => MapProductToProductResponse(product)).ToList();

                return Ok(productResponses);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProductRequest productRequest)
        {
            try
            {
                Product newProduct = MapProductRequestToProduct(productRequest);

                var category = await _categoryRepository.FindByIdAsync(productRequest.CategoryId);
                if (category == null)
                {
                    return NotFound();
                }
                var product = await _productRepository.CreateAsync(newProduct);

                ProductResponse productResponse = MapProductToProductResponse(product);

                return Ok(productResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> FindByIdAsync([FromRoute] int productId)
        {
            try
            {
                var product = await _productRepository.FindByIdAsync(productId);
                if (product == null)
                {
                    return NotFound();
                }

                return Ok(MapProductToProductResponse(product));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPut]
        [Route("{productId}")]
        public async Task<IActionResult> UpdateByIdAsync([FromRoute] int productId, [FromBody] ProductRequest productRequest)
        {
            try
            {
                var updateProduct = MapProductRequestToProduct(productRequest);

                var product = await _productRepository.UpdateByIdAsync(productId, updateProduct);

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(MapProductToProductResponse(product));
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

        [Authorize(Role.Admin)]
        [HttpDelete]
        [Route("{productId}")]
        public async Task<IActionResult> DeleteByIdAsync([FromRoute] int productId)
        {
            try
            {
                var product = await _productRepository.DeleteByIdAsync(productId);
                if (product == null)
                {
                    return NotFound();
                }

                return Ok(MapProductToProductResponse(product));
            }
            catch (Exception ex)
            {

                return Problem(ex.Message);
            }
        }

        //We are taking an product, making and sending back as a new product respons
        private ProductResponse MapProductToProductResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId
            };
        }

        //We are taking an product request, making and sending back a new product
        private Product MapProductRequestToProduct(ProductRequest productRequest)
        {
            return new Product
            {
                Name = productRequest.Name,
                Description = productRequest.Description,
                Price = productRequest.Price,
                CategoryId = productRequest.CategoryId
            };
        }
    }
}
