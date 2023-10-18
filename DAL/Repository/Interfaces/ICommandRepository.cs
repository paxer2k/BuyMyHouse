using Domain.Entities;

namespace DAL.Repository.Interfaces
{
    public interface ICommandRepository<T> where T : BaseEntity
    { 
        Task<T> CreateAsync(T entity);
        Task<bool?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
    }
}
