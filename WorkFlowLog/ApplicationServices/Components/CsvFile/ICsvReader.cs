using WorkFlowLog.ApplicationServices.Components.CsvFile.Models;

namespace WorkFlowLog.ApplicationServices.Components.CsvFile;

public interface ICsvReader
{
    List<Employee> ProcessEmployees(string filePath);

    List<Project> ProcessProjects(string filePath);

    List<Operation> ProcessOperations(string filePath);
}
