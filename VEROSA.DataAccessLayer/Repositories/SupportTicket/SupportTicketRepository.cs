using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VEROSA.Common.Enums;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.SupportTicket
{
    public class SupportTicketRepository
        : GenericRepository<Entities.SupportTicket>,
            ISupportTicketRepository
    {
        public SupportTicketRepository(DbContext ctx)
            : base(ctx) { }

        public async Task<IEnumerable<Entities.SupportTicket>> FindTicketsAsync(
            Guid? customerId,
            SupportStatus? status
        )
        {
            IQueryable<Entities.SupportTicket> q = _context
                .Set<Entities.SupportTicket>()
                .Include(t => t.Customer)
                .AsNoTracking();

            if (customerId.HasValue)
                q = q.Where(t => t.CustomerId == customerId.Value);

            if (status.HasValue)
                q = q.Where(t => t.Status == status.Value);

            return await q.OrderByDescending(t => t.UpdatedAt).ToListAsync();
        }
    }
}
