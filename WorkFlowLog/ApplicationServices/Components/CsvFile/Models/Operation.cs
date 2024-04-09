namespace WorkFlowLog.ApplicationServices.Components.CsvFile.Models;

public class Operation
{
    public string Name { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string EmployeeFirstName { get; set; }
    
    public string EmployeeLastName { get; set; }

    public string ProjectName { get; set; }
}
