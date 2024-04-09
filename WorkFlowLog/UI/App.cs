using WorkFlowLog.ApplicationServices.Services;

namespace WorkFlowLog.UI;

public class App : IApp
{
    private readonly IUserCommunication _userCommunication;
    private readonly IOperationsProvider _operationsProvider;
    private readonly IEmployeesProvider _employeesProvider;
    private readonly IProjectsProvider _projectsProvider;

    public App(IUserCommunication userCommunication,
        IOperationsProvider operationsProvider,
        IEmployeesProvider employeesProvider,
        IProjectsProvider projectsProvider
        )
    {
        _userCommunication = userCommunication;
        _operationsProvider = operationsProvider;
        _employeesProvider = employeesProvider;
        _projectsProvider = projectsProvider;
    }

    public void Run()
    {
        _userCommunication.ShowMessage("Witaj w programie WorkFlowLog");
        _userCommunication.ShowMessage("WorkFlowLog to aplikacja zaprojektowana w celu usprawnienia " +
            "śledzenia czasu i zarządzania godzinami pracy pracowników, przydzielonymi zadaniami " +
            "i harmonogramami realizacji projektów.");

        bool exitKeyPressed = false;

        while (!exitKeyPressed)
        {
            _userCommunication.ShowMenu();
            var key = _userCommunication.GetInputAndConvertToLower(string.Empty);

            switch (key)
            {
                case UserCommunication.ReadEmployeesCommand:
                    _employeesProvider.ReadAllEmployees();
                    break;

                case UserCommunication.AddEmployeeCommand:
                    _employeesProvider.AddEmployee();
                    break;

                case UserCommunication.EditEmployeeCommand:
                    _employeesProvider.EditEmployee();
                    break;

                case UserCommunication.RemoveEmployeeCommand:
                    _employeesProvider.RemoveEmployee();
                    break;

                case UserCommunication.LoadEmployeesFromCsvCommand:
                    _employeesProvider.InsertEmployees();
                    break;

                case UserCommunication.ReadProjectsCommand:
                    _projectsProvider.ReadAllProjects();
                    break;

                case UserCommunication.AddProjectCommand:
                    _projectsProvider.AddProject();
                    break;

                case UserCommunication.EditProjectCommand:
                    _projectsProvider.EditProject();
                    break;

                case UserCommunication.RemoveProjectCommand:
                    _projectsProvider.RemoveProject();
                    break;

                case UserCommunication.LoadProjectsFromCsvCommand:
                    _projectsProvider.InsertProjects();
                    break;

                case UserCommunication.ReadOperationsByEmployeeCommand:
                    _operationsProvider.ReadOperationsByEmployee();
                    break;

                case UserCommunication.ReadOperationsByProjectCommand:
                    _operationsProvider.ReadOperationsByProject();
                    break;

                case UserCommunication.ReadOperationsCommand:
                    _operationsProvider.ReadAllOperations();
                    break;

                case UserCommunication.RemoveOperationCommand:
                    _operationsProvider.RemoveOperation();
                    break;

                case UserCommunication.SetCurrentEmployeeCommand:
                    _operationsProvider.SetCurrentEmployee();
                    break;

                case UserCommunication.SetCurrentProjectCommand:
                    _operationsProvider.SetCurrentProject();
                    break;

                case UserCommunication.LoadOperationsFromCsvCommand:
                    _operationsProvider.InsertOperations();
                    break;

                case UserCommunication.AddNewOperationCommand:
                    _operationsProvider.AddNewOperation();
                    break;

                case UserCommunication.ShowCurrentEmployeeCommand:
                    _userCommunication.ShowEmployee(_operationsProvider.GetCurrentEmployee());
                    break;

                case UserCommunication.ShowCurrentProjectCommand:
                    _userCommunication.ShowProject(_operationsProvider.GetCurrentProject());
                    break;

                case UserCommunication.EditOperationCommand:
                    _operationsProvider.EditOperation();
                    break;

                case UserCommunication.StartOperationCommand:
                    _operationsProvider.StartOperation();
                    break;

                case UserCommunication.StopOperationCommand:
                    _operationsProvider.StopOperation();
                    break;

                case UserCommunication.CreateReportForEmployeeCommand:
                    _operationsProvider.CreateReportForEmployee();
                    break;

                case UserCommunication.ExitCommand:
                    exitKeyPressed = true;
                    break;

                default:
                    _userCommunication.ShowWarning("\n\nNieznana komenda!");
                    break;
            }
        }
    }
}
