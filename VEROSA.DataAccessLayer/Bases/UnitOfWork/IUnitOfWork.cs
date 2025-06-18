using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA.DataAccessLayer.Bases.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Account> Accounts { get; }

        Task<int> CommitAsync();
    }
}
