using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.Contact;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/v1/contacts")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _svc;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IContactService svc, ILogger<ContactController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetWithParams(
            [FromQuery] string? name = null,
            [FromQuery] string? email = null,
            [FromQuery] bool? isResolved = null,
            [FromQuery(Name = "sort_by")] string? sortBy = null,
            [FromQuery(Name = "sort_desc")] bool sortDesc = false,
            [FromQuery(Name = "page_number")] int pageNumber = 1,
            [FromQuery(Name = "page_size")] int pageSize = 10
        )
        {
            var page = await _svc.GetWithParamsAsync(
                name,
                email,
                isResolved,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            return Ok(
                new ApiResponse<PageResult<ContactResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Contacts retrieved successfully",
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
                _logger.LogWarning($"Contact not found with id {id}");
                return NotFound(
                    new ApiResponse<ContactResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Contact not found",
                        Data = null,
                    }
                );
            }

            return Ok(
                new ApiResponse<ContactResponse>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Contact retrieved successfully",
                    Data = dto,
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ContactRequest req)
        {
            var created = await _svc.CreateAsync(req);
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                new ApiResponse<ContactResponse>
                {
                    Code = StatusCodes.Status201Created,
                    Success = true,
                    Message = "Contact created successfully",
                    Data = created,
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ContactRequest req)
        {
            if (!await _svc.UpdateAsync(id, req))
            {
                _logger.LogWarning($"Failed to update contact with id {id}");
                return NotFound(
                    new ApiResponse<ContactResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Contact not found",
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
                _logger.LogWarning($"Failed to delete contact with id {id}");
                return NotFound(
                    new ApiResponse<ContactResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Contact not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }
    }
}
