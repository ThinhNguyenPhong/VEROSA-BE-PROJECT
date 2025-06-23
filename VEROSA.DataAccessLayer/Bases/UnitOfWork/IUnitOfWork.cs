using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Entities;
using VEROSA.DataAccessLayer.Repositories.Account;
using VEROSA.DataAccessLayer.Repositories.Address;
using VEROSA.DataAccessLayer.Repositories.BeautyService;
using VEROSA.DataAccessLayer.Repositories.BlogPost;
using VEROSA.DataAccessLayer.Repositories.Contact;
using VEROSA.DataAccessLayer.Repositories.Favorite;
using VEROSA.DataAccessLayer.Repositories.Product;
using VEROSA.DataAccessLayer.Repositories.Review;
using VEROSA.DataAccessLayer.Repositories.SupportTicket;

namespace VEROSA.DataAccessLayer.Bases.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository Accounts { get; }
        IAddressRepository Addresses { get; }
        IBeautyServiceRepository BeautyServices { get; }
        IProductCategoryRepository ProductCategories { get; }
        IProductRepository Products { get; }
        IBlogPostRepository BlogPosts { get; }
        IContactRepository Contacts { get; }
        IFavoriteRepository Favorites { get; }
        IReviewRepository Reviews { get; }
        ISupportTicketRepository SupportTickets { get; }
        Task<int> CommitAsync();
    }
}
