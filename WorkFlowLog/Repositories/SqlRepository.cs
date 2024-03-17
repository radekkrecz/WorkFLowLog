using Microsoft.EntityFrameworkCore;
using WorkFlowLog.Entities;

namespace WorkFlowLog.Repositories
{
    public class SqlRepository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly DbSet<T> _dbSet;
        private readonly DbContext _dbContext;

        public event EventHandler<T>? ItemAdded;
        public event EventHandler<T>? ItemRemoved;

        public SqlRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public void Add(T item)
        {
            _dbSet.Add(item);
            ItemAdded?.Invoke(this, item);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.OrderBy(item => item.Id).ToList();
        }

        public T? GetById(int id)
        {
            return _dbSet.Find(id);    
        }

        public void Remove(T item)
        {
            try
            {
                _dbSet.Remove(item);
                ItemRemoved?.Invoke(this, item);
            }
            catch (Exception)
            {
                throw new Exception($"Cannot remove item {item}");
            }
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
