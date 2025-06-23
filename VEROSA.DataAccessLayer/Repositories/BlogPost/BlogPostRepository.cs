using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VEROSA.Common.Enums;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.BlogPost
{
    public class BlogPostRepository : GenericRepository<Entities.BlogPost>, IBlogPostRepository
    {
        public BlogPostRepository(DbContext context)
            : base(context) { }

        public async Task<IEnumerable<Entities.BlogPost>> FindBlogPostsAsync(
            string? title,
            PostType? type,
            Guid? authorId
        )
        {
            IQueryable<Entities.BlogPost> q = _context
                .Set<Entities.BlogPost>()
                .Include(b => b.Author)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(title))
                q = q.Where(b => b.Title.Contains(title));

            if (type.HasValue)
                q = q.Where(b => b.Type == type.Value);

            if (authorId.HasValue)
                q = q.Where(b => b.AuthorId == authorId.Value);

            return await q.OrderByDescending(b => b.PublishedAt).ToListAsync();
        }
    }
}
