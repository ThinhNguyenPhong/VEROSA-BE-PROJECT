using Microsoft.EntityFrameworkCore;
using VEROSA.Common.Enums;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Account
{
    public class AuthRepository : GenericRepository<Entities.Account>, IAuthRepository
    {
        public async Task<IEnumerable<Entities.Account>> FindAccountsAsync(
            string? username,
            string? email,
            AccountRole? role,
            AccountStatus? status
        )
        {
            var query = _context.Set<Entities.Account>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(username))
                query = query.Where(a => a.Username.Contains(username));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(a => a.Email.Contains(email));

            if (role.HasValue)
                query = query.Where(a => a.Role == role.Value);

            if (status.HasValue)
                query = query.Where(a => a.Status == status.Value);

            return await query.ToListAsync();
        }

        public AuthRepository(DbContext context)
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
