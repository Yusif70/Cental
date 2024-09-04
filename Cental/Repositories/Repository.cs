using Cental.Context;
using Cental.Exceptions;
using Cental.Models;
using Cental.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cental.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly CarRentDBContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(CarRentDBContext carRentDBContext)
        {
            _context = carRentDBContext;
            _dbSet = _context.Set<T>();
        }
        public async Task<bool> AddAsync(T entity)
        {
            EntityEntry entityEntry = await _dbSet.AddAsync(entity);
            return entityEntry.State == EntityState.Added;
        }
        public async Task AddRangeAsync(IEnumerable<T> entities) => await _dbSet.AddRangeAsync(entities);
        public bool Update(T entity)
        {
            EntityEntry entityEntry = _dbSet.Update(entity);
            return entityEntry.State == EntityState.Modified;
        }
        public bool Remove(T entity)
        {
            EntityEntry entityEntry = _dbSet.Remove(entity);
            return entityEntry.State == EntityState.Deleted;
        }
        public void RemoveRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);
        public async Task<bool> RemoveAsync(int id)
        {
            T entity = await GetAsync(id);
            EntityEntry entityEntry = _dbSet.Remove(entity);
            return entityEntry.State == EntityState.Deleted;
        }
        public IQueryable<T> GetAll() => _dbSet;
        public T Get(int id)
        {
            T? entity = _dbSet.FirstOrDefault(e => e.Id == id);
            if (entity != null)
                return entity;
            throw new EntityNotFoundException($"{typeof(T).Name} tapilmadi");
        }
        public async Task<T> GetAsync(int id)
        {
            T? entity = await _dbSet.FirstOrDefaultAsync(e => e.Id == id);
            if (entity != null)
                return entity;
            throw new EntityNotFoundException($"{typeof(T).Name} tapilmadi");
        }
        public int Save() => _context.SaveChanges();
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    }
}
