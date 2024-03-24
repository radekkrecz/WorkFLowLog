using System.Text.Json;

namespace WorkFlowLog.Entities.Extensions;

public static class EntityExtentions
{
    public static T? Copy<T>(this T itemCopy) where T : IEntity
    {
        var json = JsonSerializer.Serialize<T>(itemCopy);
        return JsonSerializer.Deserialize<T>(json);
    }
}
