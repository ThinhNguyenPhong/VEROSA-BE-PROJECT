using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Review
{
    public class ReviewRepository : GenericRepository<Entities.Review>, IReviewRepository
    {
        public ReviewRepository(DbContext context)
            : base(context) { }

        public async Task<IEnumerable<Entities.Review>> FindReviewsAsync(
            Guid? productId,
            Guid? customerId,
            int? minRating,
            int? maxRating
        )
        {
            IQueryable<Entities.Review> q = _context
                .Set<Entities.Review>()
                .Include(r => r.Product)
                .Include(r => r.Customer)
                .AsNoTracking();

            if (productId.HasValue)
                q = q.Where(r => r.ProductId == productId.Value);

            if (customerId.HasValue)
                q = q.Where(r => r.CustomerId == customerId.Value);

            if (minRating.HasValue)
                q = q.Where(r => r.Rating >= minRating.Value);

            if (maxRating.HasValue)
                q = q.Where(r => r.Rating <= maxRating.Value);

            return await q.OrderByDescending(r => r.CreatedAt).ToListAsync();
        }
    }
}
