using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Contact
{
    public interface IContactRepository : IGenericRepository<Entities.Contact>
    {
        Task<IEnumerable<Entities.Contact>> FindContactsAsync(
            string? name,
            string? email,
            bool? isResolved
        );
    }
}
