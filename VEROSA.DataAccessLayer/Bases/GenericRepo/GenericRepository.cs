using Microsoft.EntityFrameworkCore;
using VEROSA.DataAccessLayer.Context;

namespace VEROSA.DataAccessLayer.Bases.GenericRepo
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        protected readonly DbContext _context;

        public GenericRepository(DbContext context) => _context = context;

        public async Task<T> GetByIdAsync(Guid id) => await _context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void Delete(T entity) => _context.Set<T>().Remove(entity);
    }
}
