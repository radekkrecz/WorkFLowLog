namespace WorkFlowLog.Data.Entities;

public class Employee : EntityBase
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public double FullTimeEmployee { get; set; }

    public override string ToString() => $"{Id}, {FirstName}, {LastName}, {FullTimeEmployee}";
}
