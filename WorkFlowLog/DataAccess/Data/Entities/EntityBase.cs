namespace WorkFlowLog.DataAccess.Data.Entities;

public class EntityBase : IEntity
{
    public int Id { get; set; }

    public bool isActive { get; set; }
}
