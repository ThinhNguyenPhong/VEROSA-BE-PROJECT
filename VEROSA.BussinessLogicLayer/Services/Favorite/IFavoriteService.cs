using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA.BussinessLogicLayer.Services.Favorite
{
    public interface IFavoriteService
    {
        Task<IEnumerable<FavoriteResponse>> GetAllAsync();
        Task<FavoriteResponse?> GetByIdAsync(Guid id);
        Task<FavoriteResponse> CreateAsync(FavoriteRequest req);
        Task<bool> DeleteAsync(Guid id);

        Task<PageResult<FavoriteResponse>> GetWithParamsAsync(
            Guid? customerId,
            Guid? productId,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        );
    }
}
