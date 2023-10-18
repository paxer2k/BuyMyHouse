using DAL.Repository.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class CommandRepository<T> : ICommandRepository<T> where T : BaseEntity
    {
        private readonly BuyMyHouseContext _context;
        private readonly DbSet<T> _entities;

        public CommandRepository(BuyMyHouseContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            _entities.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _entities.FindAsync(id);

            if (entity == null)
                return false;

            _entities.Remove(entity);

            return await _context.SaveChangesAsync() > 0; // return true if changes happened 
        }

        public async Task<bool?> UpdateAsync(T entity)
        {
            var existingEntity = await _entities.FindAsync(entity.Id);

            if (existingEntity == null)
                return false;

            _entities.Entry(existingEntity).CurrentValues.SetValues(entity);

            return await _context.SaveChangesAsync() > 0;

        }
    }
}
