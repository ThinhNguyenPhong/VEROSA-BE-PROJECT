using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.Review;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _svc;
        private readonly ILogger<ReviewController> _logger;

        public ReviewController(IReviewService svc, ILogger<ReviewController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetWithParams(
            [FromQuery] Guid? productId = null,
            [FromQuery] Guid? customerId = null,
            [FromQuery] int? minRating = null,
            [FromQuery] int? maxRating = null,
            [FromQuery(Name = "sort_by")] string? sortBy = null,
            [FromQuery(Name = "sort_desc")] bool sortDesc = false,
            [FromQuery(Name = "page_number")] int pageNumber = 1,
            [FromQuery(Name = "page_size")] int pageSize = 10
        )
        {
            var page = await _svc.GetWithParamsAsync(
                productId,
                customerId,
                minRating,
                maxRating,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            return Ok(
                new ApiResponse<PageResult<ReviewResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Reviews retrieved successfully",
                    Data = page,
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dto = await _svc.GetByIdAsync(id);
            if (dto == null)
            {
                _logger.LogWarning($"Review not found with id {id}");
                return NotFound(
                    new ApiResponse<ReviewResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Review not found",
                        Data = null,
                    }
                );
            }
            return Ok(
                new ApiResponse<ReviewResponse>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Review retrieved successfully",
                    Data = dto,
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewRequest req)
        {
            var created = await _svc.CreateAsync(req);
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                new ApiResponse<ReviewResponse>
                {
                    Code = StatusCodes.Status201Created,
                    Success = true,
                    Message = "Review created successfully",
                    Data = created,
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ReviewRequest req)
        {
            if (!await _svc.UpdateAsync(id, req))
            {
                _logger.LogWarning($"Failed to update review with id {id}");
                return NotFound(
                    new ApiResponse<ReviewResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Review not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _svc.DeleteAsync(id))
            {
                _logger.LogWarning($"Failed to delete review with id {id}");
                return NotFound(
                    new ApiResponse<ReviewResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Review not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }
    }
}
