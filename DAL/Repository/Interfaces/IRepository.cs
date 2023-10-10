using Domain.Entities;
using System.Linq.Expressions;

namespace DAL.Repository.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> CreateAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByConditionAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllByConditionAsync(Expression<Func<T, bool>> predicate);
        Task<bool?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
