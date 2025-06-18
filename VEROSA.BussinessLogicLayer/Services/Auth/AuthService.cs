using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.Common.Settings.JWT;
using VEROSA.DataAccessLayer.Bases.UnitOfWork;

namespace VEROSA.BussinessLogicLayer.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;
        private readonly PasswordHasher<VEROSA.DataAccessLayer.Entities.Account> _passwordHasher;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<JwtSettings> jwtOptions)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtSettings = jwtOptions.Value;
            _passwordHasher = new PasswordHasher<VEROSA.DataAccessLayer.Entities.Account>();
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest dto)
        {
            // 1) Map -> entity
            var account = _mapper.Map<VEROSA.DataAccessLayer.Entities.Account>(dto);

            // 2) Hash password
            account.PasswordHash = _passwordHasher.HashPassword(account, dto.Password);

            // 3) Save
            await _unitOfWork.Accounts.AddAsync(account);
            await _unitOfWork.CommitAsync();

            // 4) Generate JWT
            var authResponse = GenerateJwt(account);
            return _mapper.Map<AuthResponse>(authResponse);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest dto)
        {
            // 1) Find user
            var account = (await _unitOfWork.Accounts.GetAllAsync()).SingleOrDefault(u =>
                u.Username == dto.Username
            );
            if (account == null)
                throw new Exception("Invalid credentials");

            // 2) Verify password
            var result = _passwordHasher.VerifyHashedPassword(
                account,
                account.PasswordHash,
                dto.Password
            );
            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid credentials");

            // 3) Return token
            var authResponse = GenerateJwt(account);
            return _mapper.Map<AuthResponse>(authResponse);
        }

        private AuthResponse GenerateJwt(VEROSA.DataAccessLayer.Entities.Account account)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, account.Username),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiresInMinutes);
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: creds
            );

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expires = expires,
            };
        }
    }
}
