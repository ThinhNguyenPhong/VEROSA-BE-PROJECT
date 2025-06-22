using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.BeautyService
{
    public interface IBeautyServiceRepository : IGenericRepository<Entities.BeautyService>
    {
        Task<IEnumerable<Entities.BeautyService>> FindBeautyServicesAsync(
            string? name,
            decimal? minPrice,
            decimal? maxPrice
        );
    }
}
