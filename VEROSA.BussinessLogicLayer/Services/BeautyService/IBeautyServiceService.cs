using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA.BussinessLogicLayer.Services.BeautyService
{
    public interface IBeautyServiceService
    {
        Task<IEnumerable<BeautyServiceResponse>> GetAllAsync();
        Task<BeautyServiceResponse?> GetByIdAsync(Guid id);
        Task<BeautyServiceResponse> CreateAsync(BeautyServiceRequest request);
        Task<bool> UpdateAsync(Guid id, BeautyServiceRequest request);
        Task<bool> DeleteAsync(Guid id);

        Task<PageResult<BeautyServiceResponse>> GetWithParamsAsync(
            string? name,
            decimal? minPrice,
            decimal? maxPrice,
            string? sortBy,
            bool sortDescending,
            int pageNumber,
            int pageSize
        );
    }
}
