using System.Linq.Expressions;
using System.Numerics;
using Gym.Models;

namespace Gym.Repository.IRepository
{
    public interface IRepository<T> where T : Entity
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>,
                IOrderedQueryable<T>>? orderBy = null, string includeProperties = null,
            bool isTracking = true);
        Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, string includeProperties = null, bool isTracking = true);
        Task<T> GetByIdAsync(Guid id);
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entity);
        Task SaveAsync();
    }
}
