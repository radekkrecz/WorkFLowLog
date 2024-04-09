using WorkFlowLog.DataAccess.Data.Entities;

namespace WorkFlowLog.ApplicationServices.Components.CsvFile;

public interface ICsvWriter
{
    void WriteEmployees(string filePath, List<Employee> employees);

    void WriteProjects(string filePath, List<Project> projects);

    void WriteOperations(string filePath, List<Models.Operation> operations, TimeSpan sumOfWorkingHours);
}
