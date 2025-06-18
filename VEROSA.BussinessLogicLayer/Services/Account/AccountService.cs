using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using VEROSA.BussinessLogicLayer.PasswordHash;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Bases.UnitOfWork;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA.BussinessLogicLayer.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _hasher;

        public AccountService(IUnitOfWork uow, IMapper mapper, IPasswordHasher hasher)
        {
            _uow = uow;
            _mapper = mapper;
            _hasher = hasher;
        }

        public async Task<AccountResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _uow.Accounts.GetByUsernameAsync(request.Username) != null)
                throw new ApplicationException("Username already taken.");
            if (await _uow.Accounts.GetByEmailAsync(request.Email) != null)
                throw new ApplicationException("Email already registered.");

            var account = _mapper.Map<VEROSA.DataAccessLayer.Entities.Account>(request);
            account.Id = Guid.NewGuid();
            account.PasswordHash = _hasher.Hash(request.Password);

            await _uow.Accounts.AddAsync(account);
            await _uow.CommitAsync(); // timestamps auto-set by DbContext

            return _mapper.Map<AccountResponse>(account);
        }

        public async Task<AccountResponse> LoginAsync(LoginRequest request)
        {
            var account = await _uow.Accounts.GetByUsernameOrEmailAsync(request.UsernameOrEmail);

            if (account == null || !_hasher.Verify(account.PasswordHash, request.Password))
                throw new ApplicationException("Invalid credentials.");

            return _mapper.Map<AccountResponse>(account);
        }
    }
}
