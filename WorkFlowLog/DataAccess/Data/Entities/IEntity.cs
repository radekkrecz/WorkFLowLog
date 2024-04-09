namespace WorkFlowLog.DataAccess.Data.Entities;

public interface IEntity
{
    int Id { get; set; }

    bool isActive { get; set; }
}
