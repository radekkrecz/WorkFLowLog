using WorkFlowLog.Data.Entities;

namespace WorkFlowLog.Components.DataProviders.Interfaces;

public interface IUserCommunication
{
    void ShowMenu();

    void ShowMessage(string message);

    void ShowWarning(string message);

    string? GetInput(string message);

    string? GetInputAndConvertToLower(string message);

    void ShowProjects(List<Project> projects);

    void ShowProject(Project project);

    void ShowEmployees(List<Employee> employees);

    void ShowEmployee(Employee employee);
}
