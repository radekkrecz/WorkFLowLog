using WorkFlowLog.Data.Entities;

namespace WorkFlowLog.Components.DataProviders.Interfaces;

public interface IEmployeesProvider
{
    List<Employee> GetEmployees();

    Employee? GetEmployee(int id);

    void AddEmployee(Employee employee);

    void AddEmployees(Employee[] employees);

    void DeleteEmployee(Employee employee);

    void AddEventAddEmployee(EventHandler<Employee> eventHandler);

    void RemoveEventAddEmployee(EventHandler<Employee> eventHandler);

    public void AddEventRemoveEmployee(EventHandler<Employee> eventHandler);

    public void RemoveEventRemoveEmployee(EventHandler<Employee> eventHandler);

    void LoadEmployeesFromFile(string path);

    void SaveEmployeesToFile(string path);
}
