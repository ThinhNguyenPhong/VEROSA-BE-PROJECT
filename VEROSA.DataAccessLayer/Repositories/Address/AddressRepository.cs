using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VEROSA.DataAccessLayer.Bases.GenericRepo;

namespace VEROSA.DataAccessLayer.Repositories.Address
{
    public class AddressRepository : GenericRepository<Entities.Address>, IAddressRepository
    {
        public AddressRepository(DbContext context)
            : base(context) { }

        public async Task<IEnumerable<Entities.Address>> FindAddressesAsync(
            Guid? accountId,
            string? street,
            string? city,
            string? district,
            string? country
        )
        {
            IQueryable<Entities.Address> q = _context.Set<Entities.Address>().AsNoTracking();

            if (accountId.HasValue)
                q = q.Where(a => a.AccountId == accountId.Value);

            if (!string.IsNullOrWhiteSpace(street))
                q = q.Where(a => a.Street.Contains(street));

            if (!string.IsNullOrWhiteSpace(city))
                q = q.Where(a => a.City.Contains(city));

            if (!string.IsNullOrWhiteSpace(district))
                q = q.Where(a => a.District.Contains(district));

            if (!string.IsNullOrWhiteSpace(country))
                q = q.Where(a => a.Country.Contains(country));

            return await q.OrderByDescending(a => a.CreatedAt).ToListAsync();
        }
    }
}
