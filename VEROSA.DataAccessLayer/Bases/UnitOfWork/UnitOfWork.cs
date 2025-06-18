using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Context;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA.DataAccessLayer.Bases.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VerosaBeautyContext _context;
        private IGenericRepository<Account> _accounts;

        public UnitOfWork(VerosaBeautyContext context) => _context = context;

        public IGenericRepository<Account> Accounts =>
            _accounts ??= new GenericRepository<Account>(_context);

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
