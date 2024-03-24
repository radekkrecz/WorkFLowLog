using WorkFlowLog.Entities;
using WorkFlowLog.Repositories;

namespace WorkFlowLog.DataProviders.Interfaces;

public interface IFileDataProvider<T> where T : class, IEntity, new()
{
    List<T>? ReadFile(string path);
    void WriteFile(string path, List<T> repository);
}
