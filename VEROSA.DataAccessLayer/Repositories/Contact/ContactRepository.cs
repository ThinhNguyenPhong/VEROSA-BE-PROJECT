using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Contact
{
    public class ContactRepository : GenericRepository<Entities.Contact>, IContactRepository
    {
        public ContactRepository(DbContext context)
            : base(context) { }

        public async Task<IEnumerable<Entities.Contact>> FindContactsAsync(
            string? name,
            string? email,
            bool? isResolved
        )
        {
            IQueryable<Entities.Contact> q = _context.Set<Entities.Contact>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
                q = q.Where(c => c.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(email))
                q = q.Where(c => c.Email.Contains(email));

            if (isResolved.HasValue)
                q = q.Where(c => c.IsResolved == isResolved.Value);

            return await q.OrderByDescending(c => c.CreatedAt).ToListAsync();
        }
    }
}
