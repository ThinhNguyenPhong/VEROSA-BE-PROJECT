using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Favorite
{
    public interface IFavoriteRepository : IGenericRepository<Entities.Favorite>
    {
        Task<IEnumerable<Entities.Favorite>> FindFavoritesAsync(Guid? customerId, Guid? productId);
    }
}
