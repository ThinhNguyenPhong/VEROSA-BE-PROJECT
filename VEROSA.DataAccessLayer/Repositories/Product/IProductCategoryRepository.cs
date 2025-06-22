using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Entities;

namespace VEROSA.DataAccessLayer.Repositories.Product
{
    public interface IProductCategoryRepository : IGenericRepository<ProductCategory>
    {
        Task<IEnumerable<ProductCategory>> FindCategoriesAsync(string? name);
    }
}
