using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Context;
using VEROSA.DataAccessLayer.Entities;
using VEROSA.DataAccessLayer.Repositories.Account;

namespace VEROSA.DataAccessLayer.Bases.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VerosaBeautyContext _context;
        public IAuthRepository Accounts { get; }

        public UnitOfWork(VerosaBeautyContext context)
        {
            _context = context;
            Accounts = new AuthRepository(_context);
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
