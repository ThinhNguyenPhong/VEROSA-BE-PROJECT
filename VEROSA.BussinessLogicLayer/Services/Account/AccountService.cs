using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VEROSA.BussinessLogicLayer.PasswordHash;
using VEROSA.Common.Models.JWTSettings;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Bases.UnitOfWork;

namespace VEROSA.BussinessLogicLayer.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtSettings _jwtSettings;

        public AccountService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            IOptions<JwtSettings> jwtOptions
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtOptions.Value;
        }

        public async Task<AccountResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _unitOfWork.Accounts.GetByUsernameAsync(request.Username) != null)
                throw new ApplicationException("Username already taken.");
            if (await _unitOfWork.Accounts.GetByEmailAsync(request.Email) != null)
                throw new ApplicationException("Email already registered.");

            // map từ request => entity (đã có CreatedAt/UpdatedAt)
            var account = _mapper.Map<DataAccessLayer.Entities.Account>(request);
            account.Id = Guid.NewGuid();
            account.PasswordHash = _passwordHasher.Hash(request.Password);

            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<AccountResponse>(account);
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
        {
            var account = await _unitOfWork.Accounts.GetByUsernameOrEmailAsync(
                request.UsernameOrEmail
            );

            if (account == null || !_passwordHasher.Verify(account.PasswordHash, request.Password))
                throw new ApplicationException("Invalid credentials.");

            // 1) Tạo Claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, account.Username),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
            };

            // 2) Tạo key & creds
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3) Tạo token
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // 4) Build response
            return new AuthenticationResponse
            {
                Token = tokenString,
                Expires = expires,
                Account = _mapper.Map<AccountResponse>(account),
            };
        }
    }
}
