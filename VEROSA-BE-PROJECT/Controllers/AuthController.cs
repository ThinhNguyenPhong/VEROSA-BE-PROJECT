using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.Account;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/auths")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            try
            {
                var account = await _service.RegisterAsync(request);
                return Ok(
                    new ApiResponse<AccountResponse>
                    {
                        Code = StatusCodes.Status200OK,
                        Success = true,
                        Message = "Register Successfully",
                        Data = account,
                    }
                );
            }
            catch (ApplicationException ex)
            {
                return BadRequest(
                    new ApiResponse<object>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Success = false,
                        Message = ex.Message,
                        Data = null,
                    }
                );
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var auth = await _service.LoginAsync(request);
                return Ok(
                    new ApiResponse<AuthenticationResponse>
                    {
                        Code = StatusCodes.Status200OK,
                        Success = true,
                        Message = "Login Successfully",
                        Data = auth,
                    }
                );
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(
                    new ApiResponse<object>
                    {
                        Code = StatusCodes.Status401Unauthorized,
                        Success = false,
                        Message = ex.Message,
                        Data = null,
                    }
                );
            }
        }
    }
}
