using VEROSA.Common.Enums;
using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Entities;

public interface IAuthRepository : IGenericRepository<Account>
{
    Task<Account> GetByUsernameAsync(string username);
    Task<Account> GetByEmailAsync(string email);
    Task<Account> GetByUsernameOrEmailAsync(string input);
    Task<Account> GetByConfirmationTokenAsync(string token);
    Task<IEnumerable<Account>> FindAccountsAsync(
        string? username,
        string? email,
        AccountRole? role,
        AccountStatus? status
    );
}
