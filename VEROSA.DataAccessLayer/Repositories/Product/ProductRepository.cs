using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Product
{
    public class ProductRepository : GenericRepository<Entities.Product>, IProductRepository
    {
        public ProductRepository(DbContext context)
            : base(context) { }

        public async Task<IEnumerable<Entities.Product>> FindProductsAsync(
            Guid? categoryId,
            string? name
        )
        {
            IQueryable<Entities.Product> q = _context
                .Set<Entities.Product>()
                .Include(p => p.Category)
                .AsNoTracking();
            if (categoryId.HasValue)
                q = q.Where(p => p.CategoryId == categoryId.Value);
            if (!string.IsNullOrWhiteSpace(name))
                q = q.Where(p => p.Name.Contains(name));
            return await q.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }
    }
}
