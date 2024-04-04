namespace WorkFlowLog.Data.Entities;

public class Project : EntityBase
{
    public string Name { get; set; }

    public int OrderId { get; set; } //specjalnie generowany numer zlecenia,
                                     //nie jest powiązany z Id obiektu w klasie EntityBase
    public int CustomerId { get; set; }

    public override string ToString() => $"{Id}, {Name}, {(OrderId == 0 ? "" : OrderId)}, {CustomerId}";
    
}
