using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.Auth;
using VEROSA.Common.Models.Request;

namespace VEROSA_BE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
        {
            var authResponse = await _authService.RegisterAsync(dto);
            return Ok(authResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            var authResponse = await _authService.LoginAsync(dto);
            return Ok(authResponse);
        }
    }
}
