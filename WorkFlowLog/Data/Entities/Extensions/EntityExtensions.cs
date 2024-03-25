using System.Text.Json;
using WorkFlowLog.Data.Entities;

namespace WorkFlowLog.Data.Entities.Extensions;

public static class EntityExtentions
{
    public static T? Copy<T>(this T itemCopy) where T : IEntity
    {
        var json = JsonSerializer.Serialize(itemCopy);
        return JsonSerializer.Deserialize<T>(json);
    }
}
