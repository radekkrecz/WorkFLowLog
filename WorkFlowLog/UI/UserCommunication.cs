using Employee = WorkFlowLog.DataAccess.Data.Entities.Employee;
using Project = WorkFlowLog.DataAccess.Data.Entities.Project;

namespace WorkFlowLog.UI;

public class UserCommunication : IUserCommunication
{
    public const string StartOperationCommand = "0";
    public const string StopOperationCommand = "1";
    public const string AddNewOperationCommand = "2";
    public const string EditOperationCommand = "3";
    public const string ReadOperationsCommand = "4";
    public const string ReadOperationsByEmployeeCommand = "5";
    public const string ReadOperationsByProjectCommand = "6";
    public const string CreateReportForEmployeeCommand = "7";
    public const string RemoveOperationCommand = "8";
    public const string ReadEmployeesCommand = "9";
    public const string AddEmployeeCommand = "a";
    public const string EditEmployeeCommand = "b";
    public const string RemoveEmployeeCommand = "c";
    public const string ReadProjectsCommand = "d";
    public const string AddProjectCommand = "e";
    public const string EditProjectCommand = "f";
    public const string RemoveProjectCommand = "g";
    public const string ShowCurrentEmployeeCommand = "t";
    public const string SetCurrentEmployeeCommand = "u";
    public const string ShowCurrentProjectCommand = "v";
    public const string SetCurrentProjectCommand = "w";
    public const string LoadEmployeesFromCsvCommand = "x";
    public const string LoadProjectsFromCsvCommand = "y";
    public const string LoadOperationsFromCsvCommand = "z";

    public const string ExitCommand = "q";

    const ConsoleColor messageColor = ConsoleColor.White;
    const ConsoleColor warningColor = ConsoleColor.Red;
    const ConsoleColor employeeColor = ConsoleColor.Yellow;
    const ConsoleColor projectColor = ConsoleColor.Green;
    const ConsoleColor operationColor = ConsoleColor.Cyan;


    public void ShowMenu()
    {
        ShowMessage("\n\n*---------  Główne menu  ---------*\n");
        ShowMessage($"{StartOperationCommand} - Rozpoczęcie czynności");
        ShowMessage($"{StopOperationCommand} - Zakończenie czynności");
        ShowMessage($"{AddNewOperationCommand} - Dodanie nowej czynności");
        ShowMessage($"{EditOperationCommand} - Edycja czynności");
        ShowMessage($"{ReadOperationsCommand} - Odczyt wszystkich czynności");
        ShowMessage($"{ReadOperationsByEmployeeCommand} - Odczyt czynności pracownika");
        ShowMessage($"{ReadOperationsByProjectCommand} - Odczyt czynności w projekcie");
        ShowMessage($"{CreateReportForEmployeeCommand} - Generowanie raportu dla pracownika");
        ShowMessage($"{RemoveOperationCommand} - Usunięcie czynności");
        ShowMessage($"{ReadEmployeesCommand} - Odczyt wszystkich pracowników");
        ShowMessage($"{AddEmployeeCommand} - Dodanie nowego pracownika");
        ShowMessage($"{EditEmployeeCommand} - Edycja pracownika");
        ShowMessage($"{RemoveEmployeeCommand} - Usunięcie pracownika");
        ShowMessage($"{ReadProjectsCommand} - Odczyt wszystkich projektów");
        ShowMessage($"{AddProjectCommand} - Dodanie nowego projektu");
        ShowMessage($"{EditProjectCommand} - Edycja projektu");
        ShowMessage($"{RemoveProjectCommand} - Usunięcie projektu");
        ShowMessage($"{ShowCurrentEmployeeCommand} - Wyświetlenie bieżącego pracownika");
        ShowMessage($"{SetCurrentEmployeeCommand} - Ustawienie bieżącego pracownika");
        ShowMessage($"{ShowCurrentProjectCommand} - Wyświetlenie bieżącego projektu");
        ShowMessage($"{SetCurrentProjectCommand} - Ustawienie bieżącego projektu");
        ShowMessage($"{LoadEmployeesFromCsvCommand} - Załadowanie listy pracowników z pliku CSV");
        ShowMessage($"{LoadProjectsFromCsvCommand} - Załadowanie listy projektów z pliku CSV");
        ShowMessage($"{LoadOperationsFromCsvCommand} - Załadowanie listy czynności z pliku CSV");
        ShowMessage($"{ExitCommand} - Wyjście\n");
    }

    public string? GetInput(string message)
    {
        if (message != null && message != string.Empty)
        {
            ShowMessage(message!);
        }

        return Console.ReadLine()?.Trim();
    }

    public string? GetInputAndConvertToLower(string message)
    {
        return GetInput(message)?.ToLower();
    }

    public void ShowMessage(string message)
    {
        PrintLine(message, messageColor);
    }

    public void ShowWarning(string message)
    {
        PrintLine(message, warningColor);
    }

    public void ShowEmployee(Employee employee)
    {
        if (employee != null && employee.Id != 0 && employee.isActive)
        {
            PrintLine(employee.ToString(), employeeColor);
        }
        else
        {
            ShowWarning("Nie znaleziono pracownika");
        }
    }

    public void ShowOperation(DataAccess.Data.Entities.Operation operation)
    {
        if (operation != null && operation.Id != 0 && operation.isActive)
        {
            PrintLine(operation.ToString(), operationColor);
        }
        else
        {
            ShowWarning("Nie znaleziono operacji");
        }
    }

    public void ShowProject(Project project)
    {
        if (project != null && project.Id != 0 && project.isActive)
        {
            PrintLine(project.ToString(), projectColor);
        }
        else
        {
            ShowWarning("Nie znaleziono projektu");
        }
    }

    public void ShowEmployees(List<Employee> employees)
    {
        foreach (var employee in employees)
        {
            ShowEmployee(employee);
        }
    }

    public void ShowProjects(List<Project> projects)
    {
        foreach (var project in projects)
        {
            ShowProject(project);
        }
    }

    public void ShowOperations(List<DataAccess.Data.Entities.Operation> operations)
    {
        foreach (var operation in operations)
        {
            ShowOperation(operation);
        }
    }

    void PrintLine(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
