using System;
using WorkFlowLog.Components.CsvReader.Models;
using WorkFlowLog.Components.DataProviders.Interfaces;
using WorkFlowLog.Data.Entities;
using Employee = WorkFlowLog.Data.Entities.Employee;
using Project = WorkFlowLog.Data.Entities.Project;

namespace WorkFlowLog.Components.DataProviders;

public class UserCommunication : IUserCommunication
{
    public string? GetInput(string message, bool convertToUpper = true)
    {
        if (message != null && message != string.Empty)
        {
            ShowMessage(message!);
        }

        return convertToUpper
            ? Console.ReadLine()?.Trim().ToUpper()
            : Console.ReadLine()?.Trim();
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

    public void ShowMenu()
    {
        ShowMessage("\n\n*---------  Główne menu  ---------*\n");
        ShowMessage("1 - Odczyt wszystkich pracowników");
        ShowMessage("2 - Dodanie nowego pracownika");
        ShowMessage("3 - Usunięcie pracownika");
        ShowMessage("4 - Odczyt wszystkich projektów");
        ShowMessage("5 - Dodanie nowego projektu");
        ShowMessage("6 - Usunięcie projektu");
        ShowMessage("7 - Załadowanie listy pracowników z pliku CSV");
        ShowMessage("8 - Załadowanie listy projektów z pliku CSV");
        ShowMessage("q - Wyjście\n");
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
