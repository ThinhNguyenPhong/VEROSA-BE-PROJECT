using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.Address;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/v1/addresses")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _svc;
        private readonly ILogger<AddressController> _logger;

        public AddressController(IAddressService svc, ILogger<AddressController> logger)
        {
            _svc = svc;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetWithParams(
            [FromQuery] Guid? accountId = null,
            [FromQuery] string? street = null,
            [FromQuery] string? city = null,
            [FromQuery] string? district = null,
            [FromQuery] string? country = null,
            [FromQuery(Name = "sort_by")] string? sortBy = null,
            [FromQuery(Name = "sort_desc")] bool sortDesc = false,
            [FromQuery(Name = "page_number")] int pageNumber = 1,
            [FromQuery(Name = "page_size")] int pageSize = 10
        )
        {
            var page = await _svc.GetWithParamsAsync(
                accountId,
                street,
                city,
                district,
                country,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            return Ok(
                new ApiResponse<PageResult<AddressResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Addresses retrieved successfully",
                    Data = page,
                }
            );
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "ROLE_ADMIN,ROLE_MANAGER")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dto = await _svc.GetByIdAsync(id);
            if (dto == null)
            {
                _logger.LogWarning($"Address not found with id {id}");
                return NotFound(
                    new ApiResponse<PageResult<AddressResponse>>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Address not found",
                        Data = null,
                    }
                );
            }

            return Ok(
                new ApiResponse<PageResult<AddressResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Address retrieved successfully",
                    Data = dto,
                }
            );
        }

        [HttpPost]
        //[Authorize(Roles = "ROLE_ADMIN")]
        public async Task<IActionResult> Create([FromBody] AddressRequest request)
        {
            var created = await _svc.CreateAsync(request);
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                new ApiResponse<PageResult<AddressResponse>>
                {
                    Code = StatusCodes.Status201Created,
                    Success = true,
                    Message = "Address created successfully",
                    Data = created,
                }
            );
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "ROLE_ADMIN")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AddressRequest request)
        {
            if (!await _svc.UpdateAsync(id, request))
            {
                _logger.LogWarning($"Failed to update address with id {id}");
                return NotFound(
                    new ApiResponse<PageResult<AddressResponse>>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Address not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "ROLE_ADMIN")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!await _svc.DeleteAsync(id))
            {
                _logger.LogWarning($"Failed to delete address with id {id}");
                return NotFound(
                    new ApiResponse<PageResult<AddressResponse>>
                    {
                        Code = StatusCodes.Status404NotFound,
                        Success = false,
                        Message = "Address not found",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }
    }
}
