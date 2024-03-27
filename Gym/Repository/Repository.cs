using Gym.Models;
using Gym.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Gym.Data;

namespace Gym.Repository
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private readonly GymDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(GymDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, string includeProperties = null,
                                      bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            if (!isTracking)
                query = query.AsNoTracking();

            var result = await query.FirstOrDefaultAsync();

            return result;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>,
                                               IOrderedQueryable<T>>? orderBy = null, string includeProperties = null,
                                               bool isTracking = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            if (orderBy != null)
                query = orderBy(query);

            if (!isTracking)
                query = query.AsNoTracking();

            var result = await query.ToListAsync();

            return result;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<T> CreateAsync(T entity)
        {
            dbSet.Add(entity);
            await SaveAsync();

            return entity;
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
            await SaveAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            dbSet.Update(entity);

            await SaveAsync();
            return entity;
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
