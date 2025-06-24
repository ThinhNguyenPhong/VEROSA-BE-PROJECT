using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.Product;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [Route("api/v1/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(
            IProductCategoryService productCategoryService,
            IProductService productService,
            ILogger<ProductController> logger
        )
        {
            _productCategoryService = productCategoryService;
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("categories")]
        ////[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCategories(
            [FromQuery] string? name = null,
            [FromQuery(Name = "sort_by")] string? sortBy = null,
            [FromQuery(Name = "sort_desc")] bool sortDesc = false,
            [FromQuery(Name = "page_number")] int pageNumber = 1,
            [FromQuery(Name = "page_size")] int pageSize = 10
        )
        {
            var page = await _productCategoryService.GetWithParamsAsync(
                name,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            return Ok(
                new ApiResponse<PageResult<ProductCategoryResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Categories retrieved successfully",
                    Data = page,
                }
            );
        }

        [HttpGet("categories/{id}")]
        ////[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var dto = await _productCategoryService.GetByIdAsync(id);
            if (dto == null)
            {
                _logger.LogWarning($"Category not found with id {id}");
                return NotFound(
                    new ApiResponse<ProductCategoryResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Category not found",
                        Data = null,
                    }
                );
            }

            return Ok(
                new ApiResponse<ProductCategoryResponse>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Category retrieved successfully",
                    Data = dto,
                }
            );
        }

        [HttpPost("categories")]
        ////[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] ProductCategoryRequest req)
        {
            var created = await _productCategoryService.CreateAsync(req);
            return CreatedAtAction(
                nameof(GetCategoryById),
                new { id = created.Id },
                new ApiResponse<ProductCategoryResponse>
                {
                    Code = StatusCodes.Status201Created,
                    Success = true,
                    Message = "Category created successfully",
                    Data = created,
                }
            );
        }

        [HttpPut("categories/{id}")]
        ////[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(
            Guid id,
            [FromBody] ProductCategoryRequest req
        )
        {
            if (!await _productCategoryService.UpdateAsync(id, req))
            {
                _logger.LogWarning($"Failed to update category with id {id}");
                return NotFound(
                    new ApiResponse<ProductCategoryResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Category not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }

        [HttpDelete("categories/{id}")]
        ////[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            if (!await _productCategoryService.DeleteAsync(id))
            {
                _logger.LogWarning($"Failed to delete category with id {id}");
                return NotFound(
                    new ApiResponse<ProductCategoryResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Category not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }

        // ----- Product Endpoints -----

        [HttpGet("products")]
        ////[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProducts(
            [FromQuery] Guid? categoryId = null,
            [FromQuery] string? name = null,
            [FromQuery(Name = "sort_by")] string? sortBy = null,
            [FromQuery(Name = "sort_desc")] bool sortDesc = false,
            [FromQuery(Name = "page_number")] int pageNumber = 1,
            [FromQuery(Name = "page_size")] int pageSize = 10
        )
        {
            var page = await _productService.GetWithParamsAsync(
                categoryId,
                name,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            return Ok(
                new ApiResponse<PageResult<ProductResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Products retrieved successfully",
                    Data = page,
                }
            );
        }

        [HttpGet("products/{id}")]
        ////[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            var dto = await _productService.GetByIdAsync(id);
            if (dto == null)
            {
                _logger.LogWarning($"Product not found with id {id}");
                return NotFound(
                    new ApiResponse<ProductResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Product not found",
                        Data = null,
                    }
                );
            }

            return Ok(
                new ApiResponse<ProductResponse>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Product retrieved successfully",
                    Data = dto,
                }
            );
        }

        [HttpPost("products")]
        ////[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest req)
        {
            var created = await _productService.CreateAsync(req);
            return CreatedAtAction(
                nameof(GetProductById),
                new { id = created.Id },
                new ApiResponse<ProductResponse>
                {
                    Code = StatusCodes.Status201Created,
                    Success = true,
                    Message = "Product created successfully",
                    Data = created,
                }
            );
        }

        [HttpPut("products/{id}")]
        ////[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductRequest req)
        {
            if (!await _productService.UpdateAsync(id, req))
            {
                _logger.LogWarning($"Failed to update product with id {id}");
                return NotFound(
                    new ApiResponse<ProductResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Product not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }

        [HttpDelete("products/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            if (!await _productService.DeleteAsync(id))
            {
                _logger.LogWarning($"Failed to delete product with id {id}");
                return NotFound(
                    new ApiResponse<ProductResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Product not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }
    }
}
