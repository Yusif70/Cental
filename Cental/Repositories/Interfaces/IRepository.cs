using Cental.Models;

namespace Cental.Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<bool> AddAsync(T entity);
        public Task AddRangeAsync(IEnumerable<T> entities);
        bool Update(T entity);
        bool Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<bool> RemoveAsync(int id);
        T Get(int id);
        Task<T> GetAsync(int id);
        IQueryable<T> GetAll();
        int Save();
        Task<int> SaveAsync();
    }
}
