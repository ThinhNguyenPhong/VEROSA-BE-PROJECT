using Microsoft.AspNetCore.Mvc;
using VEROSA.BussinessLogicLayer.Services.Account;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA_BE_PROJECT.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountController(IAccountService service) => _service = service;

        [HttpPost("register")]
        public async Task<ActionResult<AccountResponse>> Register(RegisterRequest request)
        {
            try
            {
                var response = await _service.RegisterAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AccountResponse>> Login(LoginRequest request)
        {
            try
            {
                var response = await _service.LoginAsync(request);
                return Ok(response);
            }
            catch (ApplicationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountResponse>> GetById(Guid id)
        {
            // Nếu cần có UnitOfWork và Mapper, inject thêm hoặc sử dụng service mới
            // Đây là ví dụ đơn giản:
            var account = await _service.RegisterAsync(new RegisterRequest()); // Placeholder
            return Ok(account);
        }
    }
}
