using System.Text;
using System.Text.Json;
using WorkFlowLog.DataProviders.Interfaces;
using WorkFlowLog.Entities;
using WorkFlowLog.Repositories;

namespace WorkFlowLog;

public class App : IApp
{
    private readonly IOrdersProvider _ordersProvider;
    private readonly IEmployeesProvider _employeesProvider;
    private readonly IUserCommunication _userCommunication;

    public App(
        IOrdersProvider ordersProvider,
        IEmployeesProvider employeesProvider,
        IUserCommunication userCommunication
        )
    {
        _ordersProvider = ordersProvider;
        _employeesProvider = employeesProvider;
        _userCommunication = userCommunication;
    }

    const string ordersFilePath = "orders.json";
    const string employeesFilePath = "employees.json";
    const string logsEmployeeFilePath = "logEmployeeFile.txt";
    const string logsOrderFilePath = "logEmployeeFile.txt";

    public void Run()
    {
        _userCommunication.ShowMessage("Witaj w programie WorkFlowLog");
        _userCommunication.ShowMessage("WorkFlowLog to aplikacja zaprojektowana w celu usprawnienia " +
            "śledzenia czasu i zarządzania godzinami pracy pracowników, przydzielonymi zadaniami " +
            "i harmonogramami realizacji zamówień.");

        _employeesProvider.LoadEmployeesFromFile(employeesFilePath);
        _ordersProvider.LoadOrdersFromFile(ordersFilePath);

        _employeesProvider.AddEventAddEmployee(EventItemAdded);
        _employeesProvider.AddEventRemoveEmployee(EventItemRemoved);
        _ordersProvider.AddEventAddOrder(EventItemAdded);
        _ordersProvider.AddEventRemoveOrder(EventItemRemoved);


        if (_employeesProvider.GetEmployees().Count == 0)
        {
            _employeesProvider.AddEmployees(CreateEmployeeTable());
            _employeesProvider.SaveEmployeesToFile(employeesFilePath);
        }

        if (_ordersProvider.GetOrders().Count == 0)
        {
            _ordersProvider.AddOrders(CreateOrderTable());
            _ordersProvider.SaveOrdersToFile(ordersFilePath);
        }

        bool exitKeyPressed = false;

        while (!exitKeyPressed)
        {
            _userCommunication.ShowMenu();
            var key = _userCommunication.GetInput(string.Empty);

            switch (key)
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
                    _userCommunication.ShowWarning("\n\nNieznana komenda!");
                    break;
            }
        }
    }

    void EventItemAdded<T>(object? sender, T item)
    {
        if(item == null)
        {
            return;
        }

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
        if (item == null)
        {
            return;
        }

        if (item.GetType() == typeof(Employee))
        {
            File.AppendAllText(logsEmployeeFilePath, $"{DateTime.Now} - Employee deleted - [{item}]\n", Encoding.UTF8);
        }
        else
        {
            File.AppendAllText(logsOrderFilePath, $"{DateTime.Now} - Order deleted - [{item}]\n", Encoding.UTF8);
        }
    }

    void RemoveOrder()
    {
        var orderNumber = _userCommunication.GetInput("Podaj numer zamówienia do usunięcia:");    

        if (orderNumber == null)
        {
            return;
        }

        var allOrders = _ordersProvider.GetOrders();
        var orderToRemove = allOrders.FirstOrDefault
            (x => x.OrderId == int.Parse(orderNumber));

        if (orderToRemove == null)
        {
            _userCommunication.ShowWarning("Nie znaleziono zamówienia o podanym numerze");
            return;
        }

        _ordersProvider.DeleteOrder(_ordersProvider.GetOrder(orderToRemove.Id)!);

        _ordersProvider.SaveOrdersToFile(ordersFilePath);

        _userCommunication.ShowMessage($"Usunięto zamówienie nr {orderToRemove.OrderId}");
    }

    void AddOrder()
    {
        var orderName = _userCommunication.GetInput("Podaj nazwę zamówienia:", false);
        if (orderName == null || orderName == "")
        {
            _userCommunication.ShowWarning("Błąd w danych zamówienia");
            return;
        }

        var orderDescription = _userCommunication.GetInput("Podaj opis zamówienia:", false);
        if (orderDescription == null || orderDescription == "")
        {
            _userCommunication.ShowWarning("Błąd w danych zamówienia");
            return;
        }

        _userCommunication.ShowMessage("Podaj numer zamówienia:");
        var orderNumber = _userCommunication.GetInput(string.Empty);
        if (orderNumber == null)
        {
            _userCommunication.ShowWarning("Błąd w danych zamówienia");
            return;
        }
        if (int.TryParse(orderNumber, out int result))
        {
            _ordersProvider.AddOrder(new Order
            {
                Name = orderName,
                Description = orderDescription,
                OrderId = result
            });

            _ordersProvider.SaveOrdersToFile(ordersFilePath);

            _userCommunication.ShowMessage($"Dodano zamówienie {orderName}");
        }
        else
        {
            _userCommunication.ShowWarning("Błąd w danych zamówienia");
            return;
        }
    }

    void ReadAllOrders()
    {
        var orders = _ordersProvider.GetOrders();   

        _userCommunication.ShowMessage($"Ilość zamówień w bazie: {orders.Count}");

        _userCommunication.ShowOrders(orders);
    }

    void RemoveEmployee()
    {
        var peselNumber = _userCommunication.GetInput("Podaj numer PESEL pracownika do usunięcia:");

        if (peselNumber == null)
        {
            return;
        }

        var allEmployees = _employeesProvider.GetEmployees();
        var employeeToRemove = allEmployees.FirstOrDefault(x => x.PeselNumber == peselNumber);

        if (employeeToRemove == null)
        {
            _userCommunication.ShowWarning("Nie znaleziono pracownika o podanym numerze PESEL");
            return;
        }

        _employeesProvider.DeleteEmployee(_employeesProvider.GetEmployee(employeeToRemove.Id)!);

        _employeesProvider.SaveEmployeesToFile(employeesFilePath);

        _userCommunication.ShowMessage($"Usunięto pracownika {employeeToRemove}");
    }

    void AddEmployee()
    {
        var firstName = _userCommunication.GetInput("Podaj numer imię pracownika:", false);

        if (firstName == null || firstName == "")
        {
            _userCommunication.ShowWarning("Błąd w danych pracownika");
            return;
        }

        var lastName = _userCommunication.GetInput("Podaj numer nazwisko pracownika:", false);
        if (lastName == null || lastName == "")
        {
            _userCommunication.ShowWarning("Błąd w danych pracownika");
            return;
        }

        var peselNumber = _userCommunication.GetInput("Podaj numer PESEL pracownika:", false);
        if (peselNumber == null || peselNumber == "")
        {
            _userCommunication.ShowWarning("Błąd w danych pracownika");
            return;
        }

        var jobName = _userCommunication.GetInput("Podaj nazwę stanowiska pracownika:", false);
        if (jobName == null || jobName == "")
        {
            _userCommunication.ShowWarning("Błąd w danych pracownika");
            return;
        }

        var hourlyRate = _userCommunication.GetInput("Podaj stawkę godzinową pracownika:");
        if (hourlyRate == null)
        {
            _userCommunication.ShowWarning("Błąd w danych pracownika");
            return;
        }

        if (double.TryParse(hourlyRate, out double result))
        {
            _employeesProvider.AddEmployee(new Employee
            {
                FirstName = firstName,
                LastName = lastName,
                PeselNumber = peselNumber,
                JobName = jobName,
                HourlyRate = result
            });

            _employeesProvider.SaveEmployeesToFile(employeesFilePath);

            _userCommunication.ShowMessage($"Dodano pracownika {firstName} {lastName}");
        }
        else
        {
            _userCommunication.ShowWarning("Błąd w danych pracownika");
            return;
        }
    }
    void ReadAllEmployees()
    {
        var employees = _employeesProvider.GetEmployees();  

        _userCommunication.ShowMessage($"Ilość pracowników w bazie: {employees.Count}");

        _userCommunication.ShowEmployees(employees);
    }

    Employee[] CreateEmployeeTable()
    {
        return
        [
            new Employee
            {
                FirstName = "Gaja",
                LastName = "Mateńko",
                PeselNumber = "73324120622",
                JobName = "Inżynier ds. jakości",
                HourlyRate = 53.38,
                Id = 1
            },

            new Employee
            {
                FirstName = "Cezary",
                LastName = "Kowalski",
                PeselNumber = "80010100000",
                JobName = "Specjalista ds. marketingu",
                HourlyRate = 45.00,
                Id = 2
            },

            new Employee
            {
                FirstName = "Dorota",
                LastName = "Nowak",
                PeselNumber = "90020200000",
                JobName = "Analityk danych",
                HourlyRate = 60.00,
                Id = 3
            },

            new Employee
            {
                FirstName = "Cezary",
                LastName = "Kowalski",
                PeselNumber = "90020200000",
                JobName = "Kierownik projektu",
                HourlyRate = 60.00,
                Id = 4
            },
            new Employee
            {
                FirstName = "Ewa",
                LastName = "Mućko",
                PeselNumber = "03033985112",
                JobName = "Inżynier systemów",
                HourlyRate = 40.53,
                Id = 7
            }
        ];
    }

    Order[] CreateOrderTable() 
    {
        return
        [
            new Order
            {
                Name = "Zestaw naprawczy P252546 - użycie",
                Description = "Użycie 7 szt. zestaw naprawczy P252546",
                OrderId = 17,
                Id = 1
            },

            new Order
            {
                Name = "Przełącznik krańcowy P798891 - testowanie",
                Description = "Testowanie 50 szt. przełącznik krańcowy P798891",
                OrderId = 393,
                Id = 2
            },

            new Order
            {
                Name = "Pasek napędowy P777458 - wymiana",
                Description = "Wymiana 15 szt. pasek napędowy P777458",
                OrderId = 33,
                Id = 3
            },

            new Order
            {
                Name = "Przewód elastyczny P122430 - wymiana",
                Description = "Wymiana 20 szt. przewód elastyczny P122430",
                OrderId = 361,
                Id = 4
            },

            new Order
            {
                Name = "Czujnik ciśnienia P261017 - kalibracja",
                Description = "Kalibracja 36 szt. czujnik ciśnienia P261017",
                OrderId = 436,
                Id = 5
            },

            new Order
            {
                Name = "Pompa wody P275269 - konserwacja",
                Description = "Konserwacja 26 szt. pompa wody P275269",
                OrderId = 372,
                Id = 6
            },

            new Order
            {
                Name = "Przewód elastyczny P659450 - wymiana",
                Description = "Wymiana 26 szt. przewód elastyczny P659450",
                OrderId = 451,
                Id = 7
            },

            new Order
            {
                Name = "Zestaw naprawczy P203915 - użycie",
                Description = "Użycie 44 szt. zestaw naprawczy P203915",
                OrderId = 119,
                Id = 8
            },

            new Order
            {
                Name = "Rura odpływowa P164598 - wymiana",
                Description = "Wymiana 34 szt. rura odpływowa P164598",
                OrderId = 14,
                Id = 9
            },

            new Order
            {
                Name = "Kabel zasilający P586827 - montaż",
                Description = "Montaż 41 szt. kabel zasilający P586827",
                OrderId = 45,
                Id = 10
            },

            new Order
            {
                Name = "Termostat P536188 - montaż",
                Description = "Montaż 25 szt. termostat P536188",
                OrderId = 484,
                Id = 11
            },

            new Order
            {
                Name = "Włącznik światła P159950 - naprawa",
                Description = "Naprawa 7 szt. włącznik światła P159950",
                OrderId = 278,
                Id = 12
            },

            new Order
            {
                Name = "Zawór bezpieczeństwa P888469 - inspekcja",
                Description = "Inspekcja 2 szt. zawór bezpieczeństwa P888469",
                OrderId = 247,
                Id = 13
            },

            new Order
            {
                Name = "Przełącznik krańcowy P624541 - testowanie",
                Description = "Testowanie 24 szt. przełącznik krańcowy P624541",
                OrderId = 385,
                Id = 14
            },

            new Order
            {
                Name = "Przełącznik krańcowy P539348 - testowanie",
                Description = "Testowanie 23 szt. przełącznik krańcowy P539348",
                OrderId = 451,
                Id = 15
            }
        ];
    }
}
