using VEROSA.Common.Enums;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA.BussinessLogicLayer.Services.Account
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountResponse>> GetAllAsync();
        Task<AccountResponse?> GetByIdAsync(Guid id);
        Task<AccountResponse> CreateAsync(AccountRequest request);
        Task<bool> UpdateAsync(Guid id, AccountRequest request);
        Task<bool> DeleteAsync(Guid id);

        Task<PageResult<AccountResponse>> GetWithParamsAsync(
            string? username,
            string? email,
            AccountRole? role,
            AccountStatus? status,
            string? sortBy,
            bool sortDescending,
            int pageNumber,
            int pageSize
        );
    }
}
