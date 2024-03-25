namespace WorkFlowLog.Data.Entities;

public class Employee : EntityBase
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PeselNumber { get; set; }
    public string JobName { get; set; }
    public double HourlyRate { get; set; }

    public override string ToString() => $"ID: {Id}, First name:{FirstName}, Last name:{LastName}, PESEL:{PeselNumber}, Job name:{JobName}";
}
