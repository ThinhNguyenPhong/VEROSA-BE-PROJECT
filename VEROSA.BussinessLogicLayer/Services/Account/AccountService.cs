using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using VEROSA.BussinessLogicLayer.PasswordHash;
using VEROSA.BussinessLogicLayer.Services.Email;
using VEROSA.Common.Enums;
using VEROSA.Common.Models.JWTSettings;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Bases.UnitOfWork;
using JwtClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace VEROSA.BussinessLogicLayer.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService _emailer;
        private readonly string _frontendBase;

        public AccountService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPasswordHasher passwordHasher,
            IOptions<JwtSettings> jwtOptions,
            IEmailService emailer,
            IOptions<FrontendSettings> frontOpt
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtOptions.Value;
            _frontendBase = frontOpt.Value.BaseUrl;
            _emailer = emailer;
        }

        public async Task<AccountResponse> RegisterAsync(RegisterRequest req)
        {
            if (await _unitOfWork.Accounts.GetByUsernameAsync(req.Username) != null)
                throw new ApplicationException("Username đã tồn tại.");
            if (await _unitOfWork.Accounts.GetByEmailAsync(req.Email) != null)
                throw new ApplicationException("Email đã đăng ký.");

            var acct = _mapper.Map<DataAccessLayer.Entities.Account>(req);
            acct.Id = Guid.NewGuid();

            // sinh confirm token
            var token = Guid.NewGuid().ToString("N");
            acct.ConfirmationToken = token;
            acct.ConfirmationTokenExpires = DateTime.UtcNow.AddHours(24);

            await _unitOfWork.Accounts.AddAsync(acct);
            await _unitOfWork.CommitAsync();

            // gửi mail
            var link = $"{_frontendBase}/confirm?token={token}";
            var body =
                $"<p>Chào {acct.FirstName},</p>"
                + $"<p>Vui lòng bấm vào <a href=\"{link}\">đây</a> để tạo mật khẩu và kích hoạt tài khoản.</p>"
                + "<p>Link có hiệu lực 24h.</p>";
            await _emailer.SendEmailAsync(acct.Email, "Xác nhận đăng ký VEROSA", body);

            return _mapper.Map<AccountResponse>(acct);
        }

        public async Task<AuthenticationResponse> ConfirmAsync(ConfirmAccountRequest req)
        {
            var acct = await _unitOfWork.Accounts.GetByConfirmationTokenAsync(req.Token);
            if (acct == null)
                throw new ApplicationException("Token không hợp lệ hoặc đã hết hạn.");

            // tạo mật khẩu và active
            acct.PasswordHash = _passwordHasher.Hash(req.Password);
            acct.Status = AccountStatus.Active;
            acct.ConfirmationToken = null;
            acct.ConfirmationTokenExpires = null;
            acct.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CommitAsync();

            // trả token đăng nhập luôn
            var claims = new[]
            {
                new Claim(JwtClaimNames.Sub, acct.Id.ToString()),
                new Claim(JwtClaimNames.UniqueName, acct.Username),
                new Claim(ClaimTypes.Role, acct.Role.ToString() ?? string.Empty),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);
            var tokenJwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: exp,
                signingCredentials: creds
            );
            return new AuthenticationResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenJwt),
                Expires = exp,
                Account = _mapper.Map<AccountResponse>(acct),
            };
        }

        // Similar changes should be applied to the LoginAsync method:
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
                new Claim(JwtClaimNames.Sub, account.Id.ToString()),
                new Claim(JwtClaimNames.UniqueName, account.Username),
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
