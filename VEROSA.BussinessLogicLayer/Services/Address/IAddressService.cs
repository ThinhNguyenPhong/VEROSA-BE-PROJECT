using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA.BussinessLogicLayer.Services.Address
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressResponse>> GetAllAsync();
        Task<AddressResponse?> GetByIdAsync(Guid id);
        Task<AddressResponse> CreateAsync(AddressRequest request);
        Task<bool> UpdateAsync(Guid id, AddressRequest request);
        Task<bool> DeleteAsync(Guid id);

        Task<PageResult<AddressResponse>> GetWithParamsAsync(
            Guid? accountId,
            string? street,
            string? city,
            string? district,
            string? country,
            string? sortBy,
            bool sortDescending,
            int pageNumber,
            int pageSize
        );
    }
}
