using WorkFlowLog.DataAccess.Data.Entities;

namespace WorkFlowLog.ApplicationServices.Services;

public interface IEmployeesProvider
{
    List<Employee> GetEmployees();

    Employee? GetEmployeeById(int id);

    void InsertEmployees();

    void AddEmployee();

    void EditEmployee();

    void RemoveEmployee();

    void ReadAllEmployees();

    Employee LookForEmployeeByLastName(string lastName);

    Employee GetEmployeeByLastName(string lastName);

    Employee AskForEmployeeAndCheckInDb();
}
