using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Favorite
{
    public class FavoriteRepository : GenericRepository<Entities.Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(DbContext context)
            : base(context) { }

        public async Task<IEnumerable<Entities.Favorite>> FindFavoritesAsync(
            Guid? customerId,
            Guid? productId
        )
        {
            IQueryable<Entities.Favorite> q = _context
                .Set<Entities.Favorite>()
                .Include(f => f.Product)
                .AsNoTracking();

            if (customerId.HasValue)
                q = q.Where(f => f.CustomerId == customerId.Value);

            if (productId.HasValue)
                q = q.Where(f => f.ProductId == productId.Value);

            return await q.OrderByDescending(f => f.CreatedAt).ToListAsync();
        }
    }
}
