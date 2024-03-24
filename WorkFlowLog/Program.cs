using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WorkFlowLog;
using WorkFlowLog.Data;
using WorkFlowLog.DataProviders;
using WorkFlowLog.DataProviders.Interfaces;
using WorkFlowLog.Entities;
using WorkFlowLog.Repositories;

Console.Title = "WorkFlowLog";

var services = new ServiceCollection();

services.AddSingleton<IApp, App>();
services.AddSingleton<IRepository<Employee>, SqlRepository<Employee>>();
services.AddSingleton<IRepository<Order>, SqlRepository<Order>>();
services.AddSingleton<IEmployeesProvider, EmployeesProvider>();
services.AddSingleton<IOrdersProvider, OrdersProvider>();
services.AddSingleton<IUserCommunication, UserCommunication>();
services.AddSingleton<IFileDataProvider<Employee>, FileDataProvider<Employee>>();   
services.AddSingleton<IFileDataProvider<Order>, FileDataProvider<Order>>();
services.AddDbContext<WorkFlowDbContext>(option => option.UseInMemoryDatabase("WorkFlowLogDb"));

var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetService<IApp>()!;

app.Run();

/*
PrintLine("Witaj w programie WorkFlowLog", ConsoleColor.Yellow);
PrintLine("WorkFlowLog to aplikacja zaprojektowana w celu usprawnienia " +
    "śledzenia czasu i zarządzania godzinami pracy pracowników, przydzielonymi zadaniami " +
    "i harmonogramami realizacji zamówień.", ConsoleColor.White);

var employeeRepository = new SqlRepository<Employee>(new WorkFlowDbContext());
var orderRepository = new SqlRepository<Order>(new WorkFlowDbContext());

const string ordersFilePath = "orders.json";
const string employeesFilePath = "employees.json";
const string logsEmployeeFilePath = "logEmployeeFile.txt";
const string logsOrderFilePath = "logEmployeeFile.txt";

void EventItemAdded<T>(object? sender, T item)
{
    if (item.GetType() == typeof(Employee))
    {
        File.AppendAllText(logsEmployeeFilePath, $"{DateTime.Now} - Added new employee - [{item}]\n", Encoding.UTF8);
    }
    else
    {
        File.AppendAllText(logsOrderFilePath, $"{DateTime.Now} - Added new order - [{item}]\n", Encoding.UTF8);
    }
}

void EventItemRemoved<T>(object? sender, T item)
{
    if (item.GetType() == typeof(Employee))
    {
        File.AppendAllText(logsEmployeeFilePath, $"{DateTime.Now} - Employee deleted - [{item}]\n", Encoding.UTF8);
    }
    else
    {
        File.AppendAllText(logsOrderFilePath, $"{DateTime.Now} - Order deleted - [{item}]\n", Encoding.UTF8);
    }
}

GetDataFromFile(employeesFilePath, employeeRepository);
GetDataFromFile(ordersFilePath, orderRepository);

employeeRepository.ItemAdded += EventItemAdded;
employeeRepository.ItemRemoved += EventItemRemoved;
orderRepository.ItemAdded += EventItemAdded;
orderRepository.ItemRemoved += EventItemRemoved;

bool exitKeyPressed = false;

while (!exitKeyPressed)
{
    PrintLine("\n\n*---------  Główne menu  ---------*\n", ConsoleColor.Yellow);
    PrintLine("1 - Odczyt wszystkich pracowników", ConsoleColor.White);
    PrintLine("2 - Dodanie nowego pracownika", ConsoleColor.White);
    PrintLine("3 - Usunięcie pracownika", ConsoleColor.White);
    PrintLine("4 - Odczyt wszystkich zamówień", ConsoleColor.White);
    PrintLine("5 - Dodanie nowego zamówienia", ConsoleColor.White);
    PrintLine("6 - Usunięcie zamówienia", ConsoleColor.White);
    PrintLine("q - Wyjście\n", ConsoleColor.Green);

    var pressedKey = Console.ReadLine()?.Trim().ToUpper();

    switch (pressedKey)
    {
        case "1":
            ReadAllEmployees();
            break;

        case "2":
            AddEmployee();
            break;

        case "3":
            RemoveEmployee();
            break;

        case "4":
            ReadAllOrders();
            break;

        case "5":
            AddOrder();
            break;

        case "6":
            RemoveOrder();
            break;

        case "Q":
            exitKeyPressed = true;
            break;

        default:
            PrintLine("\n\nNieznana komenda!", ConsoleColor.DarkRed);
            break;
    }
}

void RemoveOrder()
{
    PrintLine("Podaj numer zamówienia do usunięcia:", ConsoleColor.White);
    var orderNumber = Console.ReadLine()?.Trim().ToUpper();

    if (orderNumber == null)
    {
        return;
    }

    var allOrders = orderRepository.GetAll().ToList();
    var orderToRemove = allOrders.FirstOrDefault
        (x => x.OrderId == int.Parse(orderNumber));

    if (orderToRemove == null)
    {
        PrintLine("Nie znaleziono zamówienia o podanym numerze", ConsoleColor.Red);
        return;
    }

    orderRepository.Remove(orderRepository.GetById(orderToRemove.Id));

    orderRepository.Save();

    SaveDataToFile(ordersFilePath, orderRepository);

    PrintLine($"Usunięto zamówienie nr {orderToRemove.OrderId}", ConsoleColor.Red);
}

void AddOrder()
{
    PrintLine("Podaj nazwę zamówienia:", ConsoleColor.White);
    var orderName = Console.ReadLine()?.Trim().ToUpper();
    if (orderName == null || orderName == "")
    {
        PrintLine("Błąd w danych zamówienia", ConsoleColor.Red);
        return;
    }

    PrintLine("Podaj opis zamówienia:", ConsoleColor.White);
    var orderDescription = Console.ReadLine()?.Trim().ToUpper();
    if (orderDescription == null || orderDescription == "")
    {
        PrintLine("Błąd w danych zamówienia", ConsoleColor.Red);
        return;
    }

    PrintLine("Podaj numer zamówienia:", ConsoleColor.White);
    var orderNumber = Console.ReadLine()?.Trim().ToUpper();
    if (orderNumber == null)
    {
        PrintLine("Błąd w danych zamówienia", ConsoleColor.Red);
        return;
    }

    if (int.TryParse(orderNumber, out int result))
    {
        orderRepository.Add(new Order
        {
            Name = orderName,
            Description = orderDescription,
            OrderId = result
        });
        orderRepository.Save();

        SaveDataToFile(ordersFilePath, orderRepository);

        PrintLine($"Dodano zamówienie {orderName}", ConsoleColor.Green);
    }
    else
    {
        PrintLine("Błąd w danych zamówienia", ConsoleColor.Red);
        return;
    }
}

void ReadAllOrders()
{
    var orders = orderRepository.GetAll().ToList();

    PrintLine($"Ilość zamówień w bazie: {orders.Count}", ConsoleColor.White);

    foreach (var order in orders)
    {
        PrintLine(order.ToString(), ConsoleColor.White);
    }
}

void RemoveEmployee()
{
    PrintLine("Podaj numer PESEL pracownika do usunięcia:", ConsoleColor.White);
    var peselNumber = Console.ReadLine()?.Trim().ToUpper();

    if (peselNumber == null)
    {
        return;
    }

    var allEmployees = employeeRepository.GetAll().ToList();
    var employeeToRemove = allEmployees.FirstOrDefault(x => x.PeselNumber == peselNumber);

    if (employeeToRemove == null)
    {
        PrintLine("Nie znaleziono pracownika o podanym numerze PESEL", ConsoleColor.Red);
        return;
    }

    employeeRepository.Remove(employeeRepository.GetById(employeeToRemove.Id));

    employeeRepository.Save();

    SaveDataToFile(employeesFilePath, employeeRepository);

    PrintLine($"Usunięto pracownika {employeeToRemove}", ConsoleColor.Red);
}

void AddEmployee()
{
    PrintLine("Podaj numer imię pracownika:", ConsoleColor.White);
    var firstName = Console.ReadLine()?.Trim().ToUpper();

    if (firstName == null || firstName == "")
    {
        PrintLine("Błąd w danych pracownika", ConsoleColor.Red);
        return;
    }

    PrintLine("Podaj numer nazwisko pracownika:", ConsoleColor.White);
    var lastName = Console.ReadLine()?.Trim().ToUpper();

    if (lastName == null || lastName == "")
    {
        PrintLine("Błąd w danych pracownika", ConsoleColor.Red);
        return;
    }

    PrintLine("Podaj numer PESEL pracownika:", ConsoleColor.White);
    var peselNumber = Console.ReadLine()?.Trim().ToUpper();

    if (peselNumber == null || peselNumber == "")
    {
        PrintLine("Błąd w danych pracownika", ConsoleColor.Red);
        return;
    }

    PrintLine("Podaj nazwę stanowiska pracownika:", ConsoleColor.White);
    var jobName = Console.ReadLine()?.Trim().ToUpper();
    if (jobName == null || jobName == "")
    {
        PrintLine("Błąd w danych pracownika", ConsoleColor.Red);
        return;
    }

    PrintLine("Podaj stawkę godzinową pracownika:", ConsoleColor.White);

    var hourlyRate = Console.ReadLine()?.Trim().ToUpper();

    if (hourlyRate == null)
    {
        PrintLine("Błąd w danych pracownika", ConsoleColor.Red);
        return;
    }

    if (double.TryParse(hourlyRate, out double result))
    {
        employeeRepository.Add(new Employee
        {
            FirstName = firstName,
            LastName = lastName,
            PeselNumber = peselNumber,
            JobName = jobName,
            HourlyRate = result
        });

        employeeRepository.Save();

        SaveDataToFile(employeesFilePath, employeeRepository);

        PrintLine($"Dodano pracownika {firstName} {lastName}", ConsoleColor.Green);
    }
    else
    {
        PrintLine("Błąd w danych pracownika", ConsoleColor.Red);
        return;
    }
}

void ReadAllEmployees()
{
    var employees = employeeRepository.GetAll().ToList();

    PrintLine($"Ilość pracowników w bazie: {employees.Count}", ConsoleColor.White);

    foreach (var employee in employees)
    {
        PrintLine(employee.ToString(), ConsoleColor.White);
    }

}

void GetDataFromFile<T>(string filePath, SqlRepository<T> repository) where T : class, IEntity, new()
{
    if (File.Exists(filePath))
    {
        var items = JsonSerializer.Deserialize<List<T>>(File.ReadAllText(filePath));

        if (items == null)
        {
            PrintLine("Błąd odczytu danych z pliku", ConsoleColor.Red);
            return;
        }

        foreach (var item in items)
        {
            repository.Add(item);
        }

        repository.Save();
    }
}
void SaveDataToFile<T>(string filePath, SqlRepository<T> repository) where T : class, IEntity, new()
{
    var items = JsonSerializer.Serialize<List<T>>(repository.GetAll().ToList());

    File.WriteAllText(filePath, items);

    PrintLine($"Zapisano:\n {items}", ConsoleColor.Green);

    foreach (var item in repository.GetAll())
    {
        Console.WriteLine(item);
    }
}

static void PrintLine(string message, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine(message);
    Console.ResetColor();
}
*/