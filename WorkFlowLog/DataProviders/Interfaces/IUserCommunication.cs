using WorkFlowLog.Entities;

namespace WorkFlowLog.DataProviders.Interfaces;

public interface IUserCommunication
{
    void ShowMenu();
    void ShowMessage(string message);
    void ShowWarning(string message);
    string? GetInput(string message, bool convertToUpper = true);
    void ShowOrders(List<Order> orders);
    void ShowOrder(Order order);
    void ShowEmployees(List<Employee> employees);
    void ShowEmployee(Employee employee);
}
