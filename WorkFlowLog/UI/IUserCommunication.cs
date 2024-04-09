using WorkFlowLog.DataAccess.Data.Entities;

namespace WorkFlowLog.UI;

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

    void ShowOperations(List<Operation> operations); 

    void ShowOperation(Operation operation);

    void ShowEmployee(Employee employee);
}
