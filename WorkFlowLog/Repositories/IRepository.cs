using WorkFlowLog.Entities;

namespace WorkFlowLog.Repositories
{
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : class, IEntity, new()
    {
    }
}
