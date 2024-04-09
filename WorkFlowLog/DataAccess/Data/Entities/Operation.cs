namespace WorkFlowLog.DataAccess.Data.Entities;

public class Operation : EntityBase
{
    public string Name { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int EmployeeId { get; set; }

    public int ProjectId { get; set; }

    public override string ToString() => $"{Id}, {Name}, {StartDate:g}, {EndDate:g}, {EmployeeId}, {ProjectId}";    
}
