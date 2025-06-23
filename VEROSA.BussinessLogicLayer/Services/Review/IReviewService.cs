using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA.BussinessLogicLayer.Services.Review
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewResponse>> GetAllAsync();
        Task<ReviewResponse?> GetByIdAsync(Guid id);
        Task<ReviewResponse> CreateAsync(ReviewRequest req);
        Task<bool> UpdateAsync(Guid id, ReviewRequest req);
        Task<bool> DeleteAsync(Guid id);

        Task<PageResult<ReviewResponse>> GetWithParamsAsync(
            Guid? productId,
            Guid? customerId,
            int? minRating,
            int? maxRating,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        );
    }
}
