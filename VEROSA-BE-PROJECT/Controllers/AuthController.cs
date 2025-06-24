using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.Account;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/v1/auths")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("registers")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var result = await _authService.RegisterAsync(req);
            return CreatedAtAction(
                nameof(Register),
                null,
                new ApiResponse<PageResult<AccountResponse>>
                {
                    Code = StatusCodes.Status201Created,
                    Success = true,
                    Message = "Account registered successfully",
                    Data = result,
                }
            );
        }

        [HttpPost("logins")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var result = await _authService.LoginAsync(req);
            return Ok(
                new ApiResponse<PageResult<AccountResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Logged in successfully",
                    Data = result,
                }
            );
        }

        [HttpPost("set-passwords")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordRequest req)
        {
            var result = await _authService.SetPasswordAsync(req);
            return Ok(
                new ApiResponse<PageResult<AccountResponse>>
                {
                    Code = StatusCodes.Status200OK,
                    Success = true,
                    Message = "Password has been set successfully",
                    Data = result,
                }
            );
        }
    }
}
