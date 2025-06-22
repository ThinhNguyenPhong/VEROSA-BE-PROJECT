using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA.BussinessLogicLayer.Services.Product
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponse>> GetAllAsync();
        Task<ProductResponse?> GetByIdAsync(Guid id);
        Task<ProductResponse> CreateAsync(ProductRequest req);
        Task<bool> UpdateAsync(Guid id, ProductRequest req);
        Task<bool> DeleteAsync(Guid id);

        Task<PageResult<ProductResponse>> GetWithParamsAsync(
            Guid? categoryId,
            string? name,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        );
    }
}
