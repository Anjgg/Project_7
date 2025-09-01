using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using System.Linq.Expressions;

namespace P7CreateRestApi.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> AddAsync(T entity);
        void Remove(T entity);
        Task<T> UpdateAsync(T entity);
        Task<int> SaveChangesAsync();
    }


    public class Repository<T> :  IRepository<T> where T : class
    {
        private readonly LocalDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(LocalDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id) =>
            await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        public void Remove(T entity) =>
            _dbSet.Remove(entity);

        public async Task<T> UpdateAsync(T entity) 
        {
            _dbSet.Update(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task<int> SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
