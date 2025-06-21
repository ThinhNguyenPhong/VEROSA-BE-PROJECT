using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Address
{
    public interface IAddressRepository : IGenericRepository<Entities.Address>
    {
        Task<IEnumerable<Entities.Address>> FindAddressesAsync(
            Guid? accountId,
            string? street,
            string? city,
            string? district,
            string? country
        );
    }
}
