namespace WorkFlowLog.Data.Entities;

public class Project : EntityBase
{
    public string Name { get; set; }

    public int OrderId { get; set; } 

    public int CustomerId { get; set; }

    public override string ToString() => $"{Id}, {Name}, {(OrderId == 0 ? "" : OrderId)}, {CustomerId}";
    
}
