using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Context;
using VEROSA.DataAccessLayer.Entities;
using VEROSA.DataAccessLayer.Repositories.Account;
using VEROSA.DataAccessLayer.Repositories.Address;
using VEROSA.DataAccessLayer.Repositories.BeautyService;

namespace VEROSA.DataAccessLayer.Bases.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VerosaBeautyContext _context;
        public IAuthRepository Accounts { get; }
        public IAddressRepository Addresses { get; }
        public IBeautyServiceRepository BeautyServices { get; }

        public UnitOfWork(VerosaBeautyContext context)
        {
            _context = context;
            Accounts = new AuthRepository(_context);
            Addresses = new AddressRepository(_context);
            BeautyServices = new BeautyServiceRepository(_context);
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
