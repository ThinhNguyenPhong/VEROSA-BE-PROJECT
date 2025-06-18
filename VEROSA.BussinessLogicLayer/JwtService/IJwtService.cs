using VEROSA.DataAccessLayer.Entities;

namespace VEROSA.BussinessLogicLayer.JwtService
{
    public interface IJwtService
    {
        string GenerateToken(Account account);
    }
}
