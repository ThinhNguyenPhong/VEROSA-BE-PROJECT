using Microsoft.EntityFrameworkCore;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Account
{
    public class AccountRepository : GenericRepository<Entities.Account>, IAccountRepository
    {
        public AccountRepository(DbContext context)
            : base(context) { }

        public async Task<Entities.Account> GetByUsernameAsync(string username) =>
            await _context.Set<Entities.Account>().FirstOrDefaultAsync(a => a.Username == username);

        public async Task<Entities.Account> GetByEmailAsync(string email) =>
            await _context.Set<Entities.Account>().FirstOrDefaultAsync(a => a.Email == email);

        public Task<Entities.Account> GetByUsernameOrEmailAsync(string input) =>
            _context
                .Set<Entities.Account>()
                .FirstOrDefaultAsync(a => a.Username == input || a.Email == input);

        public Task<Entities.Account> GetByConfirmationTokenAsync(string token) =>
            _context
                .Set<Entities.Account>()
                .FirstOrDefaultAsync(a =>
                    a.ConfirmationToken == token
                    && a.ConfirmationTokenExpires != null
                    && a.ConfirmationTokenExpires > DateTime.UtcNow
                );
    }
}
