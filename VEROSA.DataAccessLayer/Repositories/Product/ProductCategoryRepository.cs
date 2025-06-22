using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA.DataAccessLayer.Repositories.Product
{
    public class ProductCategoryRepository
        : GenericRepository<ProductCategory>,
            IProductCategoryRepository
    {
        public ProductCategoryRepository(DbContext context)
            : base(context) { }

        public async Task<IEnumerable<ProductCategory>> FindCategoriesAsync(string? name)
        {
            IQueryable<ProductCategory> q = _context.Set<ProductCategory>().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(name))
                q = q.Where(c => c.Name.Contains(name));
            return await q.OrderBy(c => c.Name).ToListAsync();
        }
    }
}
