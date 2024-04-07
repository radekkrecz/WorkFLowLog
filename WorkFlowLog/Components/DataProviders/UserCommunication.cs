using WorkFlowLog.Components.DataProviders.Interfaces;
using Employee = WorkFlowLog.Data.Entities.Employee;
using Project = WorkFlowLog.Data.Entities.Project;

namespace WorkFlowLog.Components.DataProviders;

public class UserCommunication : IUserCommunication
{
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

    public void ShowEmployee(Employee employee)
    {
        PrintLine(employee.ToString(), ConsoleColor.Yellow);
    }

    public void ShowEmployees(List<Employee> employees)
    {
        foreach (var employee in employees)
        {
            ShowEmployee(employee);
        }
    }

    public const string ExitCommand = "q";
    public const string ReadEmployeesCommand = "1";
    public const string AddEmployeeCommand = "2";
    public const string RemoveEmployeeCommand = "3";
    public const string ReadProjectsCommand = "4";
    public const string AddProjectCommand = "5";
    public const string RemoveProjectCommand = "6";
    public const string LoadEmployeesFromCsvCommand = "7";
    public const string LoadProjectsFromCsvCommand = "8";
        

    public void ShowMenu()
    {
        ShowMessage("\n\n*---------  Główne menu  ---------*\n");
        ShowMessage($"{ReadEmployeesCommand} - Odczyt wszystkich pracowników");
        ShowMessage($"{AddEmployeeCommand} - Dodanie nowego pracownika");
        ShowMessage($"{RemoveEmployeeCommand} - Usunięcie pracownika");
        ShowMessage($"{ReadProjectsCommand} - Odczyt wszystkich projektów");
        ShowMessage($"{AddProjectCommand} - Dodanie nowego projektu");
        ShowMessage($"{RemoveProjectCommand} - Usunięcie projektu");
        ShowMessage($"{LoadEmployeesFromCsvCommand} - Załadowanie listy pracowników z pliku CSV");
        ShowMessage($"{LoadProjectsFromCsvCommand} - Załadowanie listy projektów z pliku CSV");
        ShowMessage($"{ExitCommand} - Wyjście\n");
    }

    public void ShowMessage(string message)
    {
        PrintLine(message, ConsoleColor.White);
    }

    public void ShowProject(Project project)
    {
        PrintLine(project.ToString(), ConsoleColor.Green);
    }

    public void ShowProjects(List<Project> projects)
    {
        foreach (var project in projects)
        {
            ShowProject(project);
        }
    }

    public void ShowWarning(string message)
    {
        PrintLine(message, ConsoleColor.Red);
    }

    void PrintLine(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
}
