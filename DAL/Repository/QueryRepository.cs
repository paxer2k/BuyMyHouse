using DAL.Repository.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public class QueryRepository<T> : IQueryRepository<T> where T : BaseEntity
    {
        private readonly BuyMyHouseContext _context;
        private readonly DbSet<T> _entities;

        public QueryRepository(BuyMyHouseContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().IgnoreAutoIncludes().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }

        public async Task<T> GetByConditionAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.FirstOrDefaultAsync(predicate);
        }
    }
}
