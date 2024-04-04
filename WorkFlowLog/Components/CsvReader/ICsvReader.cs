using WorkFlowLog.Components.CsvReader.Models;
namespace WorkFlowLog.Components.CsvReader;

public interface ICsvReader
{
    List<Employee> ProcessEmployees(string filePath);

    List<Project> ProcessProjects(string filePath);
}
