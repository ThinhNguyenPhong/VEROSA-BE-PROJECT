using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.BlogPost;
using VEROSA.Common.Enums;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostController : ControllerBase
    {
        private readonly IBlogPostService _svc;
        private readonly ILogger<BlogPostController> _logger;

        public BlogPostController(IBlogPostService svc, ILogger<BlogPostController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetWithParams(
            [FromQuery] string? title = null,
            [FromQuery] PostType? type = null,
            [FromQuery] Guid? authorId = null,
            [FromQuery(Name = "sort_by")] string? sortBy = null,
            [FromQuery(Name = "sort_desc")] bool sortDesc = false,
            [FromQuery(Name = "page_number")] int pageNumber = 1,
            [FromQuery(Name = "page_size")] int pageSize = 10
        )
        {
            var page = await _svc.GetWithParamsAsync(
                title,
                type,
                authorId,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            return Ok(
                new ApiResponse<PageResult<BlogPostResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Blog posts retrieved successfully",
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
                _logger.LogWarning($"BlogPost not found with id {id}");
                return NotFound(
                    new ApiResponse<BlogPostResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "BlogPost not found",
                        Data = null,
                    }
                );
            }

            return Ok(
                new ApiResponse<BlogPostResponse>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "BlogPost retrieved successfully",
                    Data = dto,
                }
            );
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] BlogPostRequest req)
        {
            var created = await _svc.CreateAsync(req);
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                new ApiResponse<BlogPostResponse>
                {
                    Code = StatusCodes.Status201Created,
                    Success = true,
                    Message = "BlogPost created successfully",
                    Data = created,
                }
            );
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] BlogPostRequest req)
        {
            if (!await _svc.UpdateAsync(id, req))
            {
                _logger.LogWarning($"Failed to update BlogPost with id {id}");
                return NotFound(
                    new ApiResponse<BlogPostResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "BlogPost not found",
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
            if (!await _svc.DeleteAsync(id))
            {
                _logger.LogWarning($"Failed to delete BlogPost with id {id}");
                return NotFound(
                    new ApiResponse<BlogPostResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "BlogPost not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }
    }
}
