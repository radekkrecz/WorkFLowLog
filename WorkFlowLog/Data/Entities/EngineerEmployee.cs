namespace WorkFlowLog.Data.Entities;

public class EngineerEmployee : Employee
{
    public string LaboratoryName { get; set; }

    public override string ToString() => base.ToString() + $" (Inżynier pracowni {LaboratoryName})";
}
