using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Enums;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA.BussinessLogicLayer.Services.SupportTicket
{
    public interface ISupportTicketService
    {
        Task<IEnumerable<SupportTicketResponse>> GetAllAsync();
        Task<SupportTicketResponse?> GetByIdAsync(Guid id);
        Task<SupportTicketResponse> CreateAsync(SupportTicketRequest req);
        Task<bool> UpdateAsync(Guid id, SupportTicketRequest req);
        Task<bool> DeleteAsync(Guid id);

        Task<PageResult<SupportTicketResponse>> GetWithParamsAsync(
            Guid? customerId,
            SupportStatus? status,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        );
    }
}
