using WorkFlowLog.DataAccess.Data.Entities;

namespace WorkFlowLog.ApplicationServices.Services;

public interface IOperationsProvider
{
    void AddOperation(Operation operation);

    void RemoveOperation();

    void StartOperation();

    void StopOperation();

    void ReadAllOperations();

    void ReadOperationsByEmployee();

    void ReadOperationsByProject();

    Operation? GetCurrentlyActiveOperationByEmployee(Employee employee);

    void InsertOperations();

    void AddNewOperation(); 

    void EditOperation();

    bool SetCurrentProject();

    Project GetCurrentProject();

    bool SetCurrentEmployee();

    Employee GetCurrentEmployee();

    void CreateReportForEmployee();

    TimeSpan GetSumOfWorkingHours(List<Operation> operations);
}

