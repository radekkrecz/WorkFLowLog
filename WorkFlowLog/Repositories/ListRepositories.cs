using WorkFlowLog.Entities;

namespace WorkFlowLog.Repositories;

public class ListRepositories<T> : IRepository<T> where T : class, IEntity, new()
{
    protected readonly List<T> _items = [];

    public event EventHandler<T>? ItemAdded;
    public event EventHandler<T>? ItemRemoved;

    public void Add(T item)
    {
        item.Id = _items.Count + 1;
        _items.Add(item);
    }

    public void Save()
    {
    }

    public T? GetById(int id)
    {
        return id == 0 ? default : _items.Single(item => item.Id == id);
    }

    public IEnumerable<T> GetAll()
    {
        return _items.ToList();
    }

    public void Remove(T item)
    {
        _items.Remove(item);
    }
}
