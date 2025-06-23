using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Review
{
    public interface IReviewRepository : IGenericRepository<Entities.Review>
    {
        Task<IEnumerable<Entities.Review>> FindReviewsAsync(
            Guid? productId,
            Guid? customerId,
            int? minRating,
            int? maxRating
        );
    }
}
