using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VEROSA.Common.Enums;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Account
{
    public class AccountRepository : GenericRepository<Entities.Account>, IAccountRepository
    {
        public AccountRepository(DbContext context)
            : base(context) { }

        public async Task<IEnumerable<Entities.Account>> FindAccountsAsync(
            string? username,
            string? email,
            AccountRole? role,
            AccountStatus? status
        )
        {
            IQueryable<Entities.Account> q = _context.Set<Entities.Account>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(username))
                q = q.Where(a => a.Username.Contains(username));

            if (!string.IsNullOrWhiteSpace(email))
                q = q.Where(a => a.Email.Contains(email));

            if (role.HasValue)
                q = q.Where(a => a.Role == role.Value);

            if (status.HasValue)
                q = q.Where(a => a.Status == status.Value);

            return await q.OrderByDescending(a => a.CreatedAt).ToListAsync();
        }
    }
}
