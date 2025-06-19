using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VEROSA.BussinessLogicLayer.PasswordHash;
using VEROSA.BussinessLogicLayer.Services.Email;
using VEROSA.Common.Enums;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.Common.Models.Settings;
using VEROSA.DataAccessLayer.Bases.UnitOfWork;

namespace VEROSA.BussinessLogicLayer.Services.Account
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _hasher;
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService _emailer;
        private readonly string _frontendBase;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IPasswordHasher hasher,
            IOptions<JwtSettings> jwtOpt,
            IEmailService emailer,
            IOptions<FrontendSettings> frontOpt
        )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hasher = hasher;
            _jwtSettings = jwtOpt.Value;
            _emailer = emailer;
            _frontendBase = frontOpt.Value.BaseUrl;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _unitOfWork.Accounts.GetByUsernameAsync(request.Username) != null)
                throw new ApplicationException("Username đã tồn tại.");
            if (await _unitOfWork.Accounts.GetByEmailAsync(request.Email) != null)
                throw new ApplicationException("Email đã đăng ký.");

            var acct = _mapper.Map<DataAccessLayer.Entities.Account>(request);
            acct.Id = Guid.NewGuid();

            acct.ConfirmationToken = Guid.NewGuid().ToString("N");
            acct.ConfirmationTokenExpires = DateTime.UtcNow.AddHours(24);

            await _unitOfWork.Accounts.AddAsync(acct);
            await _unitOfWork.CommitAsync();

            // gửi mail xác nhận
            var link = $"{_frontendBase}/confirm?token={acct.ConfirmationToken}";
            var body =
                $@"
                <p>Chào {acct.FirstName},</p>
                <p>Vui lòng bấm vào <a href=""{link}"">đây</a> để tạo mật khẩu và kích hoạt tài khoản.</p>
                <p>Link có hiệu lực 24h.</p>";
            await _emailer.SendEmailAsync(acct.Email, "Xác nhận đăng ký VEROSA", body);

            return _mapper.Map<AuthResponse>(acct);
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginRequest request)
        {
            var acct = await _unitOfWork.Accounts.GetByUsernameOrEmailAsync(
                request.UsernameOrEmail
            );
            if (acct == null || !_hasher.Verify(acct.PasswordHash, request.Password))
                throw new ApplicationException("Invalid credentials.");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, acct.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, acct.Username),
                new Claim(ClaimTypes.Role, acct.Role.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);
            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new AuthenticationResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                Expires = expires,
                Account = _mapper.Map<AuthResponse>(acct),
            };
        }

        public async Task<AuthenticationResponse> SetPasswordAsync(SetPasswordRequest req)
        {
            var acct = await _unitOfWork.Accounts.GetByConfirmationTokenAsync(req.Token);
            if (acct == null)
                throw new ApplicationException("Token không hợp lệ hoặc đã hết hạn.");

            acct.PasswordHash = _hasher.Hash(req.NewPassword);
            acct.Status = AccountStatus.Active;
            acct.ConfirmationToken = null;
            acct.ConfirmationTokenExpires = null;
            acct.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CommitAsync();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, acct.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, acct.Username),
                new Claim(ClaimTypes.Role, acct.Role.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes);
            var jwt = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new AuthenticationResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                Expires = expires,
                Account = _mapper.Map<AuthResponse>(acct),
            };
        }
    }
}
