using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Entities;
using VEROSA.DataAccessLayer.Repositories.Account;
using VEROSA.DataAccessLayer.Repositories.Address;
using VEROSA.DataAccessLayer.Repositories.BeautyService;

namespace VEROSA.DataAccessLayer.Bases.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository Accounts { get; }
        IAddressRepository Addresses { get; }
        IBeautyServiceRepository BeautyServices { get; }
        Task<int> CommitAsync();
    }
}
