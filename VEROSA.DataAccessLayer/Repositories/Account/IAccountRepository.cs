using VEROSA.DataAccessLayer.Bases.GenericRepo;
using VEROSA.DataAccessLayer.Entities;

public interface IAccountRepository : IGenericRepository<Account>
{
    Task<Account> GetByUsernameAsync(string username);
    Task<Account> GetByEmailAsync(string email);
    Task<Account> GetByUsernameOrEmailAsync(string input);
}
