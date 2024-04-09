using WorkFlowLog.DataAccess.Data.Entities;

namespace WorkFlowLog.DataAccess.Data.Repositories;

public interface IWriteRepository<in T> where T : class, IEntity
{
    void Add(T item);

    void Remove(T item);

    void RemoveRange(IEnumerable<T> items);

    void Update(T item);

    void Save();
}
