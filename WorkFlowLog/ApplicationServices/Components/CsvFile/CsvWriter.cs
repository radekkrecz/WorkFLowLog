using WorkFlowLog.DataAccess.Data.Entities;

namespace WorkFlowLog.ApplicationServices.Components.CsvFile;

public class CsvWriter : ICsvWriter
{
    public void WriteEmployees(string filePath, List<Employee> employees)
    {
        using var writer = new StreamWriter(filePath);

        writer.WriteLine("FirstName;LastName;FullTimeEmployee");

        foreach (var employee in employees)
        {
            var employeeData = new Models.Employee
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                FullTimeEmployee = employee.FullTimeEmployee
            };
            writer.WriteLine($"{employeeData.FirstName};{employeeData.LastName};{employeeData.FullTimeEmployee}");
        }
    }

    public void WriteProjects(string filePath, List<Project> projects)
    {
        using var writer = new StreamWriter(filePath);
        
        writer.WriteLine("Name;OrderId;CustomerId");

        foreach (var project in projects)
        {
            var projectData = new Models.Project
            {
                Name = project.Name,
                OrderId = project.OrderId,
                CustomerId = project.CustomerId
            };
            writer.WriteLine($"{projectData.Name};{projectData.OrderId};{projectData.CustomerId}");
        }
    }

    public void WriteOperations(string filePath, List<Models.Operation> operations, TimeSpan sumOfWorkingHours)
    {
        using var writer = new StreamWriter(filePath);

        writer.WriteLine("StartDate;EndDate;Name;ProjectName;Employee");

        foreach (var operation in operations)
        {
            writer.WriteLine($"{operation.StartDate:g};{operation.EndDate:g};{operation.Name};{operation.ProjectName};{operation.EmployeeFirstName} {operation.EmployeeLastName}");
        }

        writer.WriteLine($"Suma czasu pracy: {(int)sumOfWorkingHours.TotalHours}:{sumOfWorkingHours.Minutes:D2}");
    }
}
