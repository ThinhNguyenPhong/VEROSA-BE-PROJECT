using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.Favorite;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _svc;
        private readonly ILogger<FavoriteController> _logger;

        public FavoriteController(IFavoriteService svc, ILogger<FavoriteController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetWithParams(
            [FromQuery] Guid? customerId = null,
            [FromQuery] Guid? productId = null,
            [FromQuery(Name = "sort_by")] string? sortBy = null,
            [FromQuery(Name = "sort_desc")] bool sortDesc = false,
            [FromQuery(Name = "page_number")] int pageNumber = 1,
            [FromQuery(Name = "page_size")] int pageSize = 10
        )
        {
            var page = await _svc.GetWithParamsAsync(
                customerId,
                productId,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            return Ok(
                new ApiResponse<PageResult<FavoriteResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Favorites retrieved successfully",
                    Data = page,
                }
            );
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dto = await _svc.GetByIdAsync(id);
            if (dto == null)
            {
                _logger.LogWarning($"Favorite not found with id {id}");
                return NotFound(
                    new ApiResponse<FavoriteResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Favorite not found",
                        Data = null,
                    }
                );
            }
            return Ok(
                new ApiResponse<FavoriteResponse>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Favorite retrieved successfully",
                    Data = dto,
                }
            );
        }

        [HttpPost]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([FromBody] FavoriteRequest req)
        {
            var created = await _svc.CreateAsync(req);
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                new ApiResponse<FavoriteResponse>
                {
                    Code = StatusCodes.Status201Created,
                    Success = true,
                    Message = "Favorite created successfully",
                    Data = created,
                }
            );
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _svc.DeleteAsync(id))
            {
                _logger.LogWarning($"Failed to delete favorite with id {id}");
                return NotFound(
                    new ApiResponse<FavoriteResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Favorite not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }
    }
}
