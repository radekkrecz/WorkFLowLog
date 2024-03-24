using System.Text.Json;
using WorkFlowLog.DataProviders.Interfaces;
using WorkFlowLog.Entities;

namespace WorkFlowLog.DataProviders;

public class FileDataProvider<T> : IFileDataProvider<T> where T : class, IEntity, new()
{
    public List<T>? ReadFile(string path)
    {
        if (!File.Exists(path))
        {
            return null;
        }

        var json = File.ReadAllText(path);

        if (string.IsNullOrWhiteSpace(json))
        {
            return null;
        }

        return JsonSerializer.Deserialize<List<T>>(json);
    }

    public void WriteFile(string path, List<T> repository)
    {
        var json = JsonSerializer.Serialize(repository);
        File.WriteAllText(path, json);
    }
}
