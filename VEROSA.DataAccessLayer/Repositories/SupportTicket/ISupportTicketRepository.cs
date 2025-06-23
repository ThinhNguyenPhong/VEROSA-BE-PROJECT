using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Enums;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.SupportTicket
{
    public interface ISupportTicketRepository : IGenericRepository<Entities.SupportTicket>
    {
        Task<IEnumerable<Entities.SupportTicket>> FindTicketsAsync(
            Guid? customerId,
            SupportStatus? status
        );
    }
}
