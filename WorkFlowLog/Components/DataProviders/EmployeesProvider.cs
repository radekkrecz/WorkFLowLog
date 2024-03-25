using System.Runtime.CompilerServices;
using WorkFlowLog.Components.DataProviders.Interfaces;
using WorkFlowLog.Data.Entities;
using WorkFlowLog.Data.Repositories;
using WorkFlowLog.Data.Repositories.Extensions;

namespace WorkFlowLog.Components.DataProviders;

public class EmployeesProvider : IEmployeesProvider
{
    private readonly IRepository<Employee> _repository;
    private readonly IFileDataProvider<Employee> _fileDataProvider;
    private bool _isEventAddEmployeeAdded;
    private bool _isEventRemoveEmployeeAdded;

    public EmployeesProvider(IRepository<Employee> repository, IFileDataProvider<Employee> fileDataProvider)
    {
        _repository = repository;
        _fileDataProvider = fileDataProvider;
    }

    public void AddEmployee(Employee employee)
    {
        _repository.Add(employee);
        _repository.Save();
    }

    public void AddEmployees(Employee[] employees)
    {
        _repository.AddBatch(employees);
        _repository.Save();
    }
    public void DeleteEmployee(Employee employee)
    {
        _repository.Remove(employee);
        _repository.Save();
    }

    public Employee? GetEmployee(int id)
    {
        return _repository.GetById(id);
    }

    public List<Employee> GetEmployees()
    {
        return _repository.GetAll().OrderBy(e => e.FirstName).ThenByDescending(e => e.HourlyRate).ToList();
    }

    public void AddEventAddEmployee(EventHandler<Employee> eventHandler)
    {
        if (!_isEventAddEmployeeAdded)
            _repository.ItemAdded += eventHandler;
        _isEventAddEmployeeAdded = true;
    }

    public void RemoveEventAddEmployee(EventHandler<Employee> eventHandler)
    {
        if (_isEventAddEmployeeAdded)
            _repository.ItemAdded -= eventHandler;
        _isEventAddEmployeeAdded = false;
    }

    public void AddEventRemoveEmployee(EventHandler<Employee> eventHandler)
    {
        if (!_isEventRemoveEmployeeAdded)
            _repository.ItemRemoved += eventHandler;
        _isEventRemoveEmployeeAdded = true;
    }

    public void RemoveEventRemoveEmployee(EventHandler<Employee> eventHandler)
    {
        if (_isEventRemoveEmployeeAdded)
            _repository.ItemRemoved -= eventHandler;
        _isEventRemoveEmployeeAdded = false;
    }

    public void LoadEmployeesFromFile(string path)
    {
        var employees = _fileDataProvider.ReadFile(path);

        if (employees == null)
        {
            return;
        }

        _repository.AddBatch(employees.ToArray());
    }

    public void SaveEmployeesToFile(string path)
    {
        _fileDataProvider.WriteFile(path, _repository.GetAll().ToList());
    }
}
