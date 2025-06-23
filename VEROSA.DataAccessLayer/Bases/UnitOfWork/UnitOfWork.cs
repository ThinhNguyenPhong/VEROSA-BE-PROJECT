using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Context;
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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VerosaBeautyContext _context;
        public IAuthRepository Accounts { get; }
        public IAddressRepository Addresses { get; }
        public IBeautyServiceRepository BeautyServices { get; }
        public IProductCategoryRepository ProductCategories { get; }
        public IProductRepository Products { get; }
        public IBlogPostRepository BlogPosts { get; }
        public IContactRepository Contacts { get; }
        public IFavoriteRepository Favorites { get; }
        public IReviewRepository Reviews { get; }
        public ISupportTicketRepository SupportTickets { get; }

        public UnitOfWork(VerosaBeautyContext context)
        {
            _context = context;
            Accounts = new AuthRepository(_context);
            Addresses = new AddressRepository(_context);
            BeautyServices = new BeautyServiceRepository(_context);
            ProductCategories = new ProductCategoryRepository(_context);
            Products = new ProductRepository(_context);
            BlogPosts = new BlogPostRepository(_context);
            Contacts = new ContactRepository(_context);
            Favorites = new FavoriteRepository(_context);
            Reviews = new ReviewRepository(_context);
            SupportTickets = new SupportTicketRepository(_context);
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
