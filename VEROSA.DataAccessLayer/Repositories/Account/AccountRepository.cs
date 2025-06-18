using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Context;
using VEROSA.DataAccessLayer.Entities;

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

        public async Task<Entities.Account> GetByUsernameOrEmailAsync(string input) =>
            await _context
                .Set<Entities.Account>()
                .FirstOrDefaultAsync(a => a.Username == input || a.Email == input);
    }
}
