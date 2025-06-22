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
    public interface IProductCategoryService
    {
        Task<IEnumerable<ProductCategoryResponse>> GetAllAsync();
        Task<ProductCategoryResponse?> GetByIdAsync(Guid id);
        Task<ProductCategoryResponse> CreateAsync(ProductCategoryRequest req);
        Task<bool> UpdateAsync(Guid id, ProductCategoryRequest req);
        Task<bool> DeleteAsync(Guid id);

        Task<PageResult<ProductCategoryResponse>> GetWithParamsAsync(
            string? name,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        );
    }
}
