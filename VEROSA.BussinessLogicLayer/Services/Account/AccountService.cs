using AutoMapper;
using Microsoft.Extensions.Logging;
using VEROSA.Common.Enums;
using VEROSA.Common.Exceptions;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;
using VEROSA.DataAccessLayer.Bases.UnitOfWork;

namespace VEROSA.BussinessLogicLayer.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IUnitOfWork uow, IMapper mapper, ILogger<AccountService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AccountResponse>> GetAllAsync()
        {
            try
            {
                var list = await _uow.Accounts.GetAllAsync();
                return _mapper.Map<IEnumerable<AccountResponse>>(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all accounts");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<AccountResponse?> GetByIdAsync(Guid id)
        {
            try
            {
                var entity = await _uow.Accounts.GetByIdAsync(id);
                if (entity == null)
                    return null;
                return _mapper.Map<AccountResponse>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get account by id {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<AccountResponse> CreateAsync(AccountRequest request)
        {
            try
            {
                var entity = _mapper.Map<VEROSA.DataAccessLayer.Entities.Account>(request);
                entity.Id = Guid.NewGuid();
                entity.CreatedAt = DateTime.UtcNow;
                entity.UpdatedAt = DateTime.UtcNow;

                await _uow.Accounts.AddAsync(entity);
                await _uow.CommitAsync();

                return _mapper.Map<AccountResponse>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create account");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> UpdateAsync(Guid id, AccountRequest request)
        {
            try
            {
                var entity = await _uow.Accounts.GetByIdAsync(id);
                if (entity == null)
                    return false;

                _mapper.Map(request, entity);
                entity.UpdatedAt = DateTime.UtcNow;
                _uow.Accounts.Update(entity);
                await _uow.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update account id {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _uow.Accounts.GetByIdAsync(id);
                if (entity == null)
                    return false;

                _uow.Accounts.Delete(entity);
                await _uow.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete account id {id}");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }

        public async Task<PageResult<AccountResponse>> GetWithParamsAsync(
            string? username,
            string? email,
            AccountRole? role,
            AccountStatus? status,
            string? sortBy,
            bool sortDescending,
            int pageNumber,
            int pageSize
        )
        {
            try
            {
                var all = await _uow.Accounts.FindAccountsAsync(username, email, role, status);
                if (!all.Any())
                    throw new AppException(ErrorCode.LIST_EMPTY);

                // Áp dụng sort động
                IOrderedEnumerable<VEROSA.DataAccessLayer.Entities.Account> sorted;
                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    var propInfo = typeof(VEROSA.DataAccessLayer.Entities.Account).GetProperty(
                        sortBy
                    );
                    Func<VEROSA.DataAccessLayer.Entities.Account, object?> keySel = a =>
                        propInfo != null ? propInfo.GetValue(a) : null;

                    sorted = sortDescending ? all.OrderByDescending(keySel) : all.OrderBy(keySel);
                }
                else
                {
                    sorted = all.OrderByDescending(a => a.CreatedAt);
                }

                // Paging
                var paged = sorted.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                var dtos = _mapper.Map<List<AccountResponse>>(paged);
                return new PageResult<AccountResponse>(dtos, pageSize, pageNumber, all.Count());
            }
            catch (AppException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get accounts with params");
                throw new AppException(ErrorCode.INTERNAL_SERVER_ERROR);
            }
        }
    }
}
