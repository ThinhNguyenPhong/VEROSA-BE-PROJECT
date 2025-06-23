using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA.BussinessLogicLayer.Services.Contact
{
    public interface IContactService
    {
        Task<IEnumerable<ContactResponse>> GetAllAsync();
        Task<ContactResponse?> GetByIdAsync(Guid id);
        Task<ContactResponse> CreateAsync(ContactRequest req);
        Task<bool> UpdateAsync(Guid id, ContactRequest req);
        Task<bool> DeleteAsync(Guid id);

        Task<PageResult<ContactResponse>> GetWithParamsAsync(
            string? name,
            string? email,
            bool? isResolved,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        );
    }
}
