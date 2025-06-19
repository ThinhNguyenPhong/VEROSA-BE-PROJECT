using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Entities;
using VEROSA.DataAccessLayer.Repositories.Account;

namespace VEROSA.DataAccessLayer.Bases.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository Accounts { get; }

        Task<int> CommitAsync();
    }
}
