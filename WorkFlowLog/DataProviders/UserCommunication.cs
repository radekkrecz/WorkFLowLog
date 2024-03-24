using System;
using WorkFlowLog.DataProviders.Interfaces;
using WorkFlowLog.Entities;

namespace WorkFlowLog.DataProviders;

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
        ShowMessage("4 - Odczyt wszystkich zamówień");
        ShowMessage("5 - Dodanie nowego zamówienia");
        ShowMessage("6 - Usunięcie zamówienia");
        ShowMessage("q - Wyjście\n");
    }

    public void ShowMessage(string message)
    {
        PrintLine(message, ConsoleColor.White);
    }

    public void ShowOrder(Order order)
    {
        PrintLine(order.ToString(), ConsoleColor.Green);
    }

    public void ShowOrders(List<Order> orders)
    {
        foreach (var order in orders)
        {
            ShowOrder(order);
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
