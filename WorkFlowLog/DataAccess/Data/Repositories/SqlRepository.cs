using Microsoft.EntityFrameworkCore;
using WorkFlowLog.DataAccess.Data.Entities;

namespace WorkFlowLog.DataAccess.Data.Repositories;

public class SqlRepository<T> : IRepository<T> where T : class, IEntity, new()
{
    private readonly DbSet<T> _dbSet;
    private readonly WorkFlowDbContext _dbContext;

    public event EventHandler<T>? ItemAdded;
    public event EventHandler<T>? ItemRemoved;

    public SqlRepository(WorkFlowDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbContext.Database.EnsureCreated();
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

    public IEnumerable<T> GetAllActive()
    {
        return _dbSet.Where(x => x.isActive).OrderBy(item => item.Id).ToList();
    }
    
    public T? GetById(int id)
    {
        return _dbSet.Where(item => item.Id == id && item.isActive).FirstOrDefault();
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

    public void WriteFile(string path, IRepository<T> repository)
    {
        throw new NotImplementedException();
    }

    public void RemoveRange(IEnumerable<T> items)
    {
        _dbSet.RemoveRange(items);
    }

    public void Update(T item)
    {
        _dbSet.Update(item);
    }

    public bool Any(Func<T, bool> predicate)
    {
        return _dbSet.Any(predicate);
    }   
}
