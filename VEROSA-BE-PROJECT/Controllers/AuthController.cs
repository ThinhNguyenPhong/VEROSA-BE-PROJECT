using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.Account;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/auths")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest req) =>
            CreatedAtAction(null, await _authService.RegisterAsync(req));

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(
            [FromBody] LoginRequest req
        ) => Ok(await _authService.LoginAsync(req));

        [HttpPost("set-password")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AuthenticationResponse>> SetPassword(
            [FromBody] SetPasswordRequest req
        ) => Ok(await _authService.SetPasswordAsync(req));
    }
}
