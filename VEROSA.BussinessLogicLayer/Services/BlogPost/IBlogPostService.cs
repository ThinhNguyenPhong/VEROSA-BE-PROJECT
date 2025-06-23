using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Enums;
using VEROSA.Common.Models.Pages;
using VEROSA.Common.Models.Request;
using VEROSA.Common.Models.Response;

namespace VEROSA.BussinessLogicLayer.Services.BlogPost
{
    public interface IBlogPostService
    {
        Task<IEnumerable<BlogPostResponse>> GetAllAsync();
        Task<BlogPostResponse?> GetByIdAsync(Guid id);
        Task<BlogPostResponse> CreateAsync(BlogPostRequest req);
        Task<bool> UpdateAsync(Guid id, BlogPostRequest req);
        Task<bool> DeleteAsync(Guid id);

        Task<PageResult<BlogPostResponse>> GetWithParamsAsync(
            string? title,
            PostType? type,
            Guid? authorId,
            string? sortBy,
            bool sortDesc,
            int pageNumber,
            int pageSize
        );
    }
}
