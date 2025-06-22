using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Product
{
    public interface IProductRepository : IGenericRepository<Entities.Product>
    {
        Task<IEnumerable<Entities.Product>> FindProductsAsync(Guid? categoryId, string? name);
    }
}
