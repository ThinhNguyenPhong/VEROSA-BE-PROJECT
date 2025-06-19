using VEROSA.Common.Enums;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Account
{
    public interface IAccountRepository : IGenericRepository<Entities.Account>
    {
        Task<IEnumerable<Entities.Account>> FindAccountsAsync(
            string? username,
            string? email,
            AccountRole? role,
            AccountStatus? status
        );
    }
}
