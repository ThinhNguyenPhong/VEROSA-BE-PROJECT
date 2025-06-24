using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.SupportTicket;
using VEROSA.Common.Enums;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/v1/support-tickets")]
    public class SupportTicketController : ControllerBase
    {
        private readonly ISupportTicketService _supportTicketService;
        private readonly ILogger<SupportTicketController> _logger;

        public SupportTicketController(
            ISupportTicketService supportTicketService,
            ILogger<SupportTicketController> logger
        )
        {
            _supportTicketService = supportTicketService;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetWithParams(
            [FromQuery] Guid? customerId = null,
            [FromQuery] SupportStatus? status = null,
            [FromQuery(Name = "sort_by")] string? sortBy = null,
            [FromQuery(Name = "sort_desc")] bool sortDesc = false,
            [FromQuery(Name = "page_number")] int pageNumber = 1,
            [FromQuery(Name = "page_size")] int pageSize = 10
        )
        {
            var page = await _supportTicketService.GetWithParamsAsync(
                customerId,
                status,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            return Ok(
                new ApiResponse<PageResult<SupportTicketResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Support tickets retrieved successfully",
                    Data = page,
                }
            );
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dto = await _supportTicketService.GetByIdAsync(id);
            if (dto == null)
            {
                _logger.LogWarning($"Ticket not found with id {id}");
                return NotFound(
                    new ApiResponse<SupportTicketResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Ticket not found",
                        Data = null,
                    }
                );
            }
            return Ok(
                new ApiResponse<SupportTicketResponse>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Ticket retrieved successfully",
                    Data = dto,
                }
            );
        }

        [HttpPost]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> Create([FromBody] SupportTicketRequest req)
        {
            var created = await _supportTicketService.CreateAsync(req);
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                new ApiResponse<SupportTicketResponse>
                {
                    Code = StatusCodes.Status201Created,
                    Success = true,
                    Message = "Ticket created successfully",
                    Data = created,
                }
            );
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Staff,Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SupportTicketRequest req)
        {
            if (!await _supportTicketService.UpdateAsync(id, req))
            {
                _logger.LogWarning($"Failed to update ticket with id {id}");
                return NotFound(
                    new ApiResponse<SupportTicketResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Ticket not found",
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
            if (!await _supportTicketService.DeleteAsync(id))
            {
                _logger.LogWarning($"Failed to delete ticket with id {id}");
                return NotFound(
                    new ApiResponse<SupportTicketResponse>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Ticket not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }
    }
}
