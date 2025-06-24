using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.Account;
using VEROSA.Common.Enums;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/v1/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetWithParams(
            [FromQuery] string? username,
            [FromQuery] string? email,
            [FromQuery] AccountRole? role,
            [FromQuery] AccountStatus? status,
            [FromQuery(Name = "sort_by")] string? sortBy = null,
            [FromQuery(Name = "sort_desc")] bool sortDescending = false,
            [FromQuery(Name = "page_number")] int pageNumber = 1,
            [FromQuery(Name = "page_size")] int pageSize = 10
        )
        {
            var page = await _accountService.GetWithParamsAsync(
                username,
                email,
                role,
                status,
                sortBy,
                sortDescending,
                pageNumber,
                pageSize
            );

            return Ok(
                new ApiResponse<PageResult<AccountResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Get Accounts Successfully",
                    Data = page,
                }
            );
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var dto = await _accountService.GetByIdAsync(id);
            if (dto == null)
            {
                _logger.LogWarning($"Account not found with id {id}");
                return NotFound(
                    new ApiResponse<PageResult<AccountResponse>>
                    {
                        Code = StatusCodes.Status200OK,
                        Success = true,
                        Message = "Can't Find Accounts",
                        Data = null,
                    }
                );
            }

            return Ok(
                new ApiResponse<PageResult<AccountResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Get Accounts successfully",
                    Data = dto,
                }
            );
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] AccountRequest request)
        {
            var created = await _accountService.CreateAsync(request);
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                new ApiResponse<PageResult<AccountResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Accounts Create Successfully",
                    Data = created,
                }
            );
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] AccountRequest request)
        {
            if (!await _accountService.UpdateAsync(id, request))
            {
                _logger.LogWarning($"Failed to update account with id {id}");
                return NotFound(
                    new ApiResponse<PageResult<AccountResponse>>
                    {
                        Code = StatusCodes.Status200OK,
                        Success = true,
                        Message = "Accounts Update Successfully",
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
            if (!await _accountService.DeleteAsync(id))
            {
                _logger.LogWarning($"Failed to delete account with id {id}");
                return NotFound(
                    new ApiResponse<PageResult<AccountResponse>>
                    {
                        Code = StatusCodes.Status200OK,
                        Success = true,
                        Message = "Accounts Delete Successfully",
                        Data = null,
                    }
                );
            }
            return NoContent();
        }
    }
}
