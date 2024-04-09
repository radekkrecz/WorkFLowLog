using WorkFlowLog.DataAccess.Data.Entities;

namespace WorkFlowLog.DataAccess.Data.Repositories;

public interface IReadRepository<out T> where T : class, IEntity
{
    IEnumerable<T> GetAll();

    IEnumerable<T> GetAllActive();

    T? GetById(int id);

    bool Any(Func<T, bool> predicate);
}
