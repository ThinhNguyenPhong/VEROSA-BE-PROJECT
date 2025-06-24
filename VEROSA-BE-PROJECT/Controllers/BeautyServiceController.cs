using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.BeautyService;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/v1/beauty-services")]
    public class BeautyServiceController : ControllerBase
    {
        private readonly IBeautyServiceService _beautyServiceService;
        private readonly ILogger<BeautyServiceController> _logger;

        public BeautyServiceController(
            IBeautyServiceService beautyServiceService,
            ILogger<BeautyServiceController> logger
        )
        {
            _beautyServiceService = beautyServiceService;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetWithParams(
            [FromQuery] string? name = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery(Name = "sort_by")] string? sortBy = null,
            [FromQuery(Name = "sort_desc")] bool sortDesc = false,
            [FromQuery(Name = "page_number")] int pageNumber = 1,
            [FromQuery(Name = "page_size")] int pageSize = 10
        )
        {
            var page = await _beautyServiceService.GetWithParamsAsync(
                name,
                minPrice,
                maxPrice,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            return Ok(
                new ApiResponse<PageResult<BeautyServiceResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Beauty services retrieved successfully",
                    Data = page,
                }
            );
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dto = await _beautyServiceService.GetByIdAsync(id);
            if (dto == null)
            {
                _logger.LogWarning($"BeautyService not found with id {id}");
                return NotFound(
                    new ApiResponse<PageResult<BeautyServiceResponse>>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "BeautyService not found",
                        Data = null,
                    }
                );
            }

            return Ok(
                new ApiResponse<PageResult<BeautyServiceResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "BeautyService retrieved successfully",
                    Data = dto,
                }
            );
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] BeautyServiceRequest req)
        {
            var created = await _beautyServiceService.CreateAsync(req);
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                new ApiResponse<PageResult<BeautyServiceResponse>>
                {
                    Code = StatusCodes.Status201Created,
                    Success = true,
                    Message = "BeautyService created successfully",
                    Data = created,
                }
            );
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BeautyServiceRequest req)
        {
            if (!await _beautyServiceService.UpdateAsync(id, req))
            {
                _logger.LogWarning($"Failed to update BeautyService with id {id}");
                return NotFound(
                    new ApiResponse<PageResult<BeautyServiceResponse>>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "BeautyService not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _beautyServiceService.DeleteAsync(id))
            {
                _logger.LogWarning($"Failed to delete BeautyService with id {id}");
                return NotFound(
                    new ApiResponse<PageResult<BeautyServiceResponse>>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "BeautyService not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }
    }
}
