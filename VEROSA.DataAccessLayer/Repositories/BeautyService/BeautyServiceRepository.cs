using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.BeautyService
{
    public class BeautyServiceRepository
        : GenericRepository<Entities.BeautyService>,
            IBeautyServiceRepository
    {
        public BeautyServiceRepository(DbContext context)
            : base(context) { }

        public async Task<IEnumerable<Entities.BeautyService>> FindBeautyServicesAsync(
            string? name,
            decimal? minPrice,
            decimal? maxPrice
        )
        {
            IQueryable<Entities.BeautyService> q = _context
                .Set<Entities.BeautyService>()
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
                q = q.Where(s => s.Name.Contains(name));

            if (minPrice.HasValue)
                q = q.Where(s => s.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                q = q.Where(s => s.Price <= maxPrice.Value);

            return await q.OrderByDescending(s => s.CreatedAt).ToListAsync();
        }
    }
}
