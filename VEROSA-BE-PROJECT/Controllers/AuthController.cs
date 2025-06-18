using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.Account;
using VEROSA.Common.Models.ApiResponse;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _svc;

        public AuthController(IAccountService svc) => _svc = svc;

        [HttpPost("register")]
        public async Task<ActionResult<AccountResponse>> Register([FromBody] RegisterRequest req) =>
            CreatedAtAction(null, await _svc.RegisterAsync(req));

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(
            [FromBody] LoginRequest req
        ) => Ok(await _svc.LoginAsync(req));

        [HttpPost("set-password")]
        [ProducesResponseType(typeof(AuthenticationResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<AuthenticationResponse>> SetPassword(
            [FromBody] SetPasswordRequest req
        ) => Ok(await _svc.SetPasswordAsync(req));
    }
}
