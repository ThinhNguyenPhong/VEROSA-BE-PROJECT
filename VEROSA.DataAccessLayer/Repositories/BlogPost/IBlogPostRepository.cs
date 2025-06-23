using VEROSA.Common.Enums;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.BlogPost
{
    public interface IBlogPostRepository : IGenericRepository<Entities.BlogPost>
    {
        Task<IEnumerable<Entities.BlogPost>> FindBlogPostsAsync(
            string? title,
            PostType? type,
            Guid? authorId
        );
    }
}
